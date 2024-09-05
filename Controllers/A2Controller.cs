using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using a2.Data;
using a2.Models;
using System.Security.Claims;
using a2.Dtos;

namespace a2.Controllers{
    [Route("api")]
    [ApiController]
    public class A2Controllers : Controller{
        private readonly IA2Repo _repo;

        public A2Controllers(IA2Repo repo){
            _repo = repo;
        }

        [HttpPost("Register")]
        public ActionResult<string> Register(User user){
            var addUser = _repo.GetUser(user);
            if (addUser == null){
                _repo.AddUser(user);
                string message = "User successfully registered.";
                return Ok(message);

            } else {
                string message = $"UserName {user.UserName} is not available.";
                return Ok(message);
            
            }
        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("PurchaseSign/{ID}")]
        public ActionResult<PurchaseOutput> PurchaseSign(string ID){
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst(ClaimTypes.Name);
            string userName = c.Value.ToString();

            var user = _repo.GetUserbyUserName(userName);
            var organiser = _repo.GetOrganizer(userName);

            if (user == null && organiser == null){
                return Forbid();           
            }

            Sign signExists = _repo.GetSign(ID);

            if (signExists == null){
                string message = $"Sign {ID} not found";
                return BadRequest(message);
            }
            PurchaseOutput output = new PurchaseOutput{UserNameOutput = user.UserName, SignID = signExists.Id};
            return Ok(output);         
        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "OrganiserOnly")]
        [HttpPost("AddEvent")]
        public ActionResult<EventInput> AddEvent(EventInput eventInput){
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst(ClaimTypes.Name);
            string userName = c.Value.ToString();

            bool startCorrect = DateTime.TryParseExact(eventInput.Start, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out _);
            bool endCorrect = DateTime.TryParseExact(eventInput.End, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out _);
            if (startCorrect == true && endCorrect == true){
                Event event1 = new Event{Start = eventInput.Start, End = eventInput.End, Summary = eventInput.Summary, Description = eventInput.Description, Location = eventInput.Location};
                _repo.AddEvent(event1);
                return Ok("Success");
            } else if (startCorrect == true && endCorrect == false){
                string message = "The format of End should be yyyyMMddTHHmmssZ.";
                return BadRequest(message);
            } else if (startCorrect == false && endCorrect == true){
                string message = "The format of Start should be yyyyMMddTHHmmssZ.";
                return BadRequest(message);
            } else {
                string message = "The format of Start and End should be yyyyMMddTHHmmssZ.";
                return BadRequest(message);
            }
        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "AuthOnly")]
        [HttpGet("EventCount")]
        public int EventCount(){
            return _repo.GetCount();
        }

        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "AuthOnly")]
        [HttpGet("Event/{ID}")]
        public ActionResult Event(int ID){
            var eventExists = _repo.GetEvent(ID);
            if (eventExists == null){
                return BadRequest($"Event {ID} does not exist.");
            } else {
                Response.Headers.Add("Content-Type", "text/calendar; charset=utf-8");
                return Ok(eventExists);
            }
        }
    }
}
