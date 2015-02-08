using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mCalendar.Filters;
using mCalendar.Models;

namespace mCalendar.Controllers
{
    [Authorize]
    public class ScheduleController : ApiController
    {
        private ScheduleContext db = new ScheduleContext(); //needs to be replaced by repository for UT

        // GET api/Schedule
        public IEnumerable<ScheduleDto> GetSchedules()
        {
            return db.Schedules.Include("Events")
                .Where(u => u.UserId == User.Identity.Name)
                .OrderByDescending(u => u.Id)
                .AsEnumerable()
                .Select(schedule => new ScheduleDto(schedule));
        }

        // GET api/Schedule/5
        public ScheduleDto GetSchedule(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (schedule.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return new ScheduleDto(schedule);
        }

        // PUT api/Schedule/5
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage PutSchedule(int id, ScheduleDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != scheduleDto.ScheduleId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Schedule schedule = scheduleDto.ToEntity();
            if (db.Entry(schedule).Entity.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            db.Entry(schedule).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Schedule
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage PostSchedule(ScheduleDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            scheduleDto.UserId = User.Identity.Name;
            Schedule schedule = scheduleDto.ToEntity();
            db.Schedules.Add(schedule);
            db.SaveChanges();
            scheduleDto.ScheduleId = schedule.Id;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, scheduleDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = scheduleDto.ScheduleId }));
            return response;
        }

        // DELETE api/Schedule/5
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage DeleteTodoList(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (db.Entry(schedule).Entity.UserId != User.Identity.Name)
            {
                // Trying to delete a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var scheduleDto = new ScheduleDto(schedule);
            db.Schedules.Remove(schedule);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, scheduleDto);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}