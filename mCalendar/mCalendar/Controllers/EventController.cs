using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mCalendar.Filters;
using mCalendar.Models;

namespace mCalendar.Controllers
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class EventController : ApiController
    {
        private ScheduleContext db = new ScheduleContext(); //needs to be replaced by repository for UT

        // PUT api/Event/5
        public HttpResponseMessage PutEvent(int id, EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != eventDto.EventId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Event ev = eventDto.ToEntity();
            Schedule schedule = db.Schedules.Find(ev.ScheduleId);
            if (schedule == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (schedule.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // Need to detach to avoid duplicate primary key exception when SaveChanges is called
            db.Entry(schedule).State = EntityState.Detached;
            db.Entry(ev).State = EntityState.Modified;

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

        // POST api/Event
        public HttpResponseMessage PostEvent(EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Schedule schedule = db.Schedules.Find(eventDto.ScheduleId);
            if (schedule == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (schedule.UserId != User.Identity.Name)
            {
                // Trying to add a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            Event ev = eventDto.ToEntity();

            // Need to detach to avoid loop reference exception during JSON serialization
            db.Entry(schedule).State = EntityState.Detached;
            db.Events.Add(ev);
            db.SaveChanges();
            eventDto.EventId = ev.Id;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, eventDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = eventDto.EventId }));
            return response;
        }

        // DELETE api/Event/5
        public HttpResponseMessage DeleteEvent(int id)
        {
            Event ev = db.Events.Find(id);
            if (ev == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Schedule schedule = db.Schedules.Find(ev.ScheduleId);
            if (schedule.UserId != User.Identity.Name)
            {
                // Trying to delete a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            EventDto eventDto = new EventDto(ev);
            db.Events.Remove(ev);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, eventDto);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}