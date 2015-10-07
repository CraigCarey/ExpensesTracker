﻿using ExpenseTracker.Repository;
using ExpenseTracker.Repository.Factories;
using System;
using System.Linq;
using System.Web.Http;
using Marvin.JsonPatch;
using System.Net;
using ExpenseTracker.API.Helpers;
using System.Web.Http.Routing;
using System.Web;

namespace ExpenseTracker.API.Controllers
{
    public class ExpenseGroupsController : ApiController
    {
        // When a class implements an interface, it can be used through a reference to that interface
        // ExpenseTrackerEFRepository implements IExpenseTrackerRepository
        IExpenseTrackerRepository _repository;
        ExpenseGroupFactory _expenseGroupFactory = new ExpenseGroupFactory();

        const int maxPageSize = 10;

        public ExpenseGroupsController()
        {
            _repository = new ExpenseTrackerEFRepository(new Repository.Entities.ExpenseTrackerContext());
        }

        public ExpenseGroupsController(IExpenseTrackerRepository repository)
        {
            _repository = repository;
        }    

        // retrieve a sorted & paged list of resources
        // parameters passed as query strings
        [Route("api/expensegroups", Name = "ExpenseGroupsList")]
        public IHttpActionResult Get(string sort = "id", string status = null, string userId = null, int page = 1, int pageSize = maxPageSize)
        {
            try
            {
                int statusId = -1;
                if (status != null)
                {
                    switch (status.ToLower())
                    {
                        case "open": statusId = 1;
                            break;
                        case "confirmed": statusId = 2;
                            break;
                        case "processed": statusId = 3;
                            break;
                        default:
                            break;
                    }
                }

                var expenseGroups = _repository.GetExpenseGroups()
                                    .ApplySort(sort)
                                    // only return ExpenseGroups that conform to the statuscode & userId passed in
                                    .Where(eg => (statusId == -1 || eg.ExpenseGroupStatusId == statusId))
                                    .Where(eg => (userId == null || eg.UserId == userId));

                // restrict pageSize
                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                // calculate data for metadata
                var totalCount = expenseGroups.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // creates new links from current route
                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("ExpenseGroupsList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        sort = sort,
                        status = status,
                        userId = userId
                    }) : "";

                var nextLink = page < totalPages ? urlHelper.Link("ExpenseGroupsList",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        sort = sort,
                        status = status,
                        userId = userId
                    }) : "";

                // Anonymous type, provides a convenient way to encapsulate a set of read-only properties into a single object
                // without having to explicitly define a type first
                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                // Add the paginationHeader to the current response headers
                HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

                // return statuscode 200 containing the list of ExpenseGroups, mapped using the factory to their DTO models
                return Ok(expenseGroups                     // already sorted and filtered
                            .Skip(pageSize * (page - 1))    // skip the pages we don't need
                            .Take(pageSize)                 // take the number of items a page contains
                            .ToList()
                            .Select(eg => _expenseGroupFactory.CreateExpenseGroup(eg)));

            }
            catch (Exception)
            {
                // statuscode 500, for when the client can't help
                return InternalServerError();
            }
        }

        // retrieve a single resource, rather than a list
        public IHttpActionResult Get(int id)
        {
            try
            {
                var expenseGroup = _repository.GetExpenseGroup(id);
                
                if (expenseGroup == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_expenseGroupFactory.CreateExpenseGroup(expenseGroup));
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // Create a resource using POST, parses ExpenseGroup from request body
        [HttpPost]
        public IHttpActionResult Post([FromBody] DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                {
                    return BadRequest();
                }

                // map the DTO to an entity we can use in our repository
                var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);

                // try to insert the entity, returns a status code and the entity created
                var result = _repository.InsertExpenseGroup(eg);

                if (result.Status == RepositoryActionStatus.Created)
                {
                    var newExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);

                    return Created(Request.RequestUri + "/" + newExpenseGroup.Id.ToString(), newExpenseGroup);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // update a resource using a PUT request
        public IHttpActionResult Put(int id, [FromBody]DTO.ExpenseGroup expenseGroup)
        {
            try
            {
                if (expenseGroup == null)
                {
                    return BadRequest();                    
                }

                // map
                var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);

                var result = _repository.UpdateExpenseGroup(eg);

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to DTO
                    var updatedExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);
                    return Ok(updatedExpenseGroup);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<DTO.ExpenseGroup> expenseGroupPatchDocument)
        {
            try
            {
                if (expenseGroupPatchDocument == null)
                {
                    return BadRequest();
                }

                // first need to get ExpenseGroup that we want to update
                var expenseGroup = _repository.GetExpenseGroup(id);
                if (expenseGroup == null)
                {
                    return NotFound();
                }

                // map
                var eg = _expenseGroupFactory.CreateExpenseGroup(expenseGroup);

                // apply changes to the DTO
                expenseGroupPatchDocument.ApplyTo(eg);

                // map the DTO with applied changes to the entity, & update
                var result = _repository.UpdateExpenseGroup(_expenseGroupFactory.CreateExpenseGroup(eg));

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to DTO
                    var patchedExpenseGroup = _expenseGroupFactory.CreateExpenseGroup(result.Entity);
                    return Ok(patchedExpenseGroup);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var result = _repository.DeleteExpenseGroup(id);

                if (result.Status == RepositoryActionStatus.Deleted)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else if (result.Status == RepositoryActionStatus.NotFound)
                {
                    return NotFound();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}