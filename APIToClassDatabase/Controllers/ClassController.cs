using APIToClassDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIToClassDatabase.Controllers
{
    [Route("api/[controller]/")]
    public class ClassController : Controller
    {
        private CoursesContext databaseConnection;

        public ClassController(CoursesContext context)
        {
            this.databaseConnection = context;
        }

        /// <summary>
        /// Get all assignments/tests that are due
        /// </summary>
        /// <remarks>
        /// returns a List of all assignments/ tests that are due
        /// </remarks>
        /// <returns>an IEnumerable of ClassTracker</returns>
        [HttpGet("GetEverythingDue")]
        [ProducesResponseType(typeof(IEnumerable<ClassTracker>), 200)]
        public IEnumerable<ClassTracker> GetAll()
        {
            return databaseConnection.ClassTracker
                        .OrderBy(c => c.Course)
                            .ToList();
        }

        /// <summary>
        /// Post a new Item thats due to the database
        /// </summary>
        /// <remarks>
        /// Creating a new Item due and adding it to the database
        /// </remarks>
        /// <response code="400">if the class passed in is null</response>
        /// <response code="500">if there was a problem saving the item to the database</response>
        /// <response code="201">if the class was successfully created and added</response>
        /// <param name="newClass">the new item that is going to be added to the database</param>
        /// <returns>A status code and an object</returns>
        [HttpPost("AddUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ClassTracker), 201)]
        public IActionResult PostClass([FromBody] ClassTracker newClass)
        {
            if(newClass == null)
            {
                return StatusCode(400, "The new class must not be null");
            }

            databaseConnection.ClassTracker.Add(newClass);
            try
            {
                databaseConnection.SaveChanges();
            }
            catch(DbUpdateException dbe)
            {
                return StatusCode(500, dbe.Message);
            }

            return StatusCode(201, newClass);
        }

        /// <summary>
        /// Patch an existing Item in the database
        /// </summary>
        /// <remarks>
        /// Updates an existing Item
        /// </remarks>
        /// <response code="404">if the item couldn't be found</response>
        /// <response code="500">if the item couldn't be saved to the database properly</response>
        /// <response code="204">the item was successfully saved</response>
        /// <param name="newDateDue">The new date that the Item is due</param>
        /// <param name="newImportance">The new level of importance</param>
        /// <param name="courseID">The unique ID of the course, this is used to retrieve the course from the database</param>
        /// <returns>a Status Code and an object</returns>
        [HttpPatch("PatchUser")]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ClassTracker), 204)]
        public IActionResult PatchClass([FromQuery] string newDateDue, [FromQuery] string newImportance, [FromQuery] int courseID)
        {
            var retrieveClass = databaseConnection.ClassTracker
                                    .Where(x => x.Id == courseID)
                                        .FirstOrDefault();

            if(retrieveClass == null)
            {
                return StatusCode(404, "Course with the provided ID wasn't found");
            }

            if(newDateDue != retrieveClass.DateDue)
            {
                retrieveClass.DateDue = newDateDue;
            }

            if(newImportance != retrieveClass.Importance)
            {
                retrieveClass.Importance = newImportance;
            }

            try
            {
                databaseConnection.SaveChanges();
            }
            catch(DbUpdateException dbe)
            {
                return StatusCode(500, dbe.Message);
            }

            return StatusCode(204, retrieveClass);
        }
    }
}
