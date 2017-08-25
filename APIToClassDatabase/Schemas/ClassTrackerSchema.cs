using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using APIToClassDatabase.Models;

namespace APIToClassDatabase.Schemas
{
    public class ClassTrackerSchema : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if(context.SystemType == typeof(ClassTracker))
            {
                model.Example = new ClassTracker
                {
                    Id = 12,
                    Course = "SE 2205B",
                    Semester = "1st",
                    DateDue = "Sept 27th",
                    Importance = "High"
                };
            }
        }
    }
}
