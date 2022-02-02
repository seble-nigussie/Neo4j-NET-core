using HR.Models;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        
            private readonly IGraphClient _client;

            public EmployeeController(IGraphClient client)
            {
                _client = client;
            }

            [HttpPost]
            public async Task<IActionResult> CreateEmployee([FromBody] Employee emp)
            {
                await _client.Cypher.Create("(e:Employee $emp)")
                                    .WithParam("emp", emp)
                                    .ExecuteWithoutResultsAsync();

                return Ok();
            }

            [HttpGet("{eid}/assignemployee/{did}/")]
            public async Task<IActionResult> AssignDepartment(int did, int eid)
            {

                await _client.Cypher.Match("(d:Department), (e:Employee)")
                                    .Where((Department d, Employee e) => d.id == did && e.id == eid)
                                    .Create("(d)-[r:hasEmployee]->(e)")
                                    .ExecuteWithoutResultsAsync();

                return Ok();
            }

    }
}
