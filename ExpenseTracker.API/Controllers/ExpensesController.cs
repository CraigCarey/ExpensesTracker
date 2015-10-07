using ExpenseTracker.Repository;
using ExpenseTracker.Repository.Factories;
using Marvin.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using ExpenseTracker.API.Helpers;

namespace ExpenseTracker.API.Controllers
{
    [RoutePrefix("api")]
    public class ExpensesController : ApiController
    {
        IExpenseTrackerRepository _repository;
        ExpenseFactory _expenseFactory = new ExpenseFactory();

        const int maxPageSize = 10;

        public ExpensesController()
        {
            _repository = new ExpenseTrackerEFRepository(new Repository.Entities.ExpenseTrackerContext());
        }

        public ExpensesController(IExpenseTrackerRepository repository)
        {
            _repository = repository;
        }

        // api/expensegroups/1/expenses
        [Route("expensegroups/{expenseGroupId}/expenses", Name = "ExpensesForGroup")]
        public IHttpActionResult Get(int expenseGroupId, string fields = null, string sort = "date", int page = 1, int pageSize = maxPageSize)
        {
            try
            {

                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                }

                var expenses = _repository.GetExpenses(expenseGroupId);

                if (expenses == null)
                {
                    // 404 - expenseGroup doesn't exist
                    return NotFound();
                }

                // restrict pagesize
                if (pageSize > maxPageSize)
                {
                    pageSize = maxPageSize;
                }

                // calculate data for metadata
                var totalCount = expenses.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);

                var prevLink = page > 1 ? urlHelper.Link("ExpensesForGroup",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        expenseGroupId = expenseGroupId,
                        fields = fields,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("ExpensesForGroup",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        expenseGroupId = expenseGroupId,
                        fields = fields,
                        sort = sort
                    }) : "";


                var paginationHeader = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    previousPageLink = prevLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
                
                var expensesResult = expenses
                    .ApplySort(sort)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList()
                    .Select(exp => _expenseFactory.CreateDataShapedObject(exp, lstOfFields));


                return Ok(expensesResult);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        } 
        
        // gets a specific expense, maps to 2 separate URIs, default version is 1
        [VersionedRoute("expensegroups/{expenseGroupId}/expenses/{id}", 1)]
        [VersionedRoute("expenses/{id}", 1)]
        public IHttpActionResult Get(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;

                // if expenseGroup wasn't passed in, just get expense
                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    // if expenseGroup was passed in, only return an expense if the group and id are valid
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);

                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }
                }
                                
                if (expense != null)
                {
                    var returnValue = _expenseFactory.CreateExpense(expense);
                    return Ok(returnValue);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // gets a specific expense, maps to 2 separate URIs, default version is 2
        [VersionedRoute("expensegroups/{expenseGroupId}/expenses/{id}", 2)]
        [VersionedRoute("expenses/{id}", 2)]
        public IHttpActionResult GetV2(int id, int? expenseGroupId = null)
        {
            try
            {
                Repository.Entities.Expense expense = null;

                // if expenseGroup wasn't passed in, just get expense
                if (expenseGroupId == null)
                {
                    expense = _repository.GetExpense(id);
                }
                else
                {
                    // if expenseGroup was passed in, only return an expense if the group and id are valid
                    var expensesForGroup = _repository.GetExpenses((int)expenseGroupId);

                    if (expensesForGroup != null)
                    {
                        expense = expensesForGroup.FirstOrDefault(eg => eg.Id == id);
                    }
                }

                if (expense != null)
                {
                    var returnValue = _expenseFactory.CreateExpense(expense);
                    return Ok(returnValue);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("expenses/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {

                var result = _repository.DeleteExpense(id);

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

        [Route("expenses")]
        public IHttpActionResult Post([FromBody]DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                // map
                var exp = _expenseFactory.CreateExpense(expense);

                var result = _repository.InsertExpense(exp);
                if (result.Status == RepositoryActionStatus.Created)
                {
                    // map to dto
                    var newExp = _expenseFactory.CreateExpense(result.Entity);
                    return Created<DTO.Expense>(Request.RequestUri + "/" + newExp.Id.ToString(), newExp);
                }

                return BadRequest();

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        
        [Route("expenses/{id}")]
        public IHttpActionResult Put(int id, [FromBody]DTO.Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest();
                }

                // map
                var exp = _expenseFactory.CreateExpense(expense);

                var result = _repository.UpdateExpense(exp);
                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to dto
                    var updatedExpense = _expenseFactory.CreateExpense(result.Entity);
                    return Ok(updatedExpense);
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
        
        [Route("expenses/{id}")]
        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<DTO.Expense> expensePatchDocument)
        {
            try
            {
                // find 
                if (expensePatchDocument == null)
                {
                    return BadRequest(); 
                }

                var expense = _repository.GetExpense(id);
                if (expense == null)
                {
                    return NotFound();
                }

                // map
                var exp = _expenseFactory.CreateExpense(expense);

                // apply changes to the DTO
                expensePatchDocument.ApplyTo(exp);

                // map the DTO with applied changes to the entity, & update
                var result = _repository.UpdateExpense(_expenseFactory.CreateExpense(exp));

                if (result.Status == RepositoryActionStatus.Updated)
                {
                    // map to dto
                    var updatedExpense = _expenseFactory.CreateExpense(result.Entity);
                    return Ok(updatedExpense);
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