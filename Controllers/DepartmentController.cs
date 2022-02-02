using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4jClient;

namespace HR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IGraphClient _client;

        public DepartmentController(IGraphClient client)
        {
            _client = client;
        }

       [HttpGet]
       public async Task<IActionResult> Get(){
             var departments = await _client.Cypher.Match("(n: Department)")
                                                   .Return(n => n.As<Department>()).ResultsAsync;

            return Ok(departments);
       }

       [HttpGet("{id}")]
       public async Task<IActionResult> GetById(int id){
             var departments = await _client.Cypher.Match("(d:Department)")
                                                   .Where((Department d) => d.id == id)
                                                   .Return(d => d.As<Department>()).ResultsAsync;

            return Ok(departments.LastOrDefault());
       }

       [HttpPost]
       public async Task<IActionResult> Create([FromBody]Department dept){
           await _client.Cypher.Create("(d:Department $dept)")
                               .WithParam("dept", dept)
                               .ExecuteWithoutResultsAsync();

            return Ok();
       }

       [HttpPut("{id}")]
       public async Task<IActionResult> Update(int id, [FromBody]Department dept){
            await _client.Cypher.Match("(d:Department)")
                                .Where((Department d) => d.id == id)
                                .Set("d = $dept")
                                .WithParam("dept", dept)
                                .ExecuteWithoutResultsAsync();
           return Ok();
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> Delete(int id){
            await  _client.Cypher.Match("(d:Department)")
                                 .Where((Department d) => d.id == id)
                                 .Delete("d")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

       }
        
    }
}
