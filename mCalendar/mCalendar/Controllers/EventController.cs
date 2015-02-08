using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mCalendar.Filters;
using mCalendar.Models;
using mCalendar.Models.Repositories;

namespace mCalendar.Controllers
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class EventController : ApiController
    {
        private readonly IEventRepository _eventRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public EventController()
        {
            _eventRepository = new EFEventRepository();
            _scheduleRepository = new EFScheduleRepository();
        }

        public EventController(IEventRepository eventRepository, IScheduleRepository scheduleRepository)
        {
            _eventRepository = eventRepository;
            _scheduleRepository = scheduleRepository;
        }

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
            Schedule schedule = _scheduleRepository.GetById(ev.ScheduleId);
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
            _scheduleRepository.Detach(schedule);

            try
            {
                _eventRepository.Save(ev);
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

            Schedule schedule = _scheduleRepository.GetById(eventDto.ScheduleId);
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
            _scheduleRepository.Detach(schedule);

            _eventRepository.Save(ev);
            eventDto.EventId = ev.Id;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, eventDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = eventDto.EventId }));
            return response;
        }

        // DELETE api/Event/5
        public HttpResponseMessage DeleteEvent(int id)
        {
            Event ev = _eventRepository.GetById(id);
            if (ev == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Schedule schedule = _scheduleRepository.GetById(ev.ScheduleId);
            if (schedule.UserId != User.Identity.Name)
            {
                // Trying to delete a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            EventDto eventDto = new EventDto(ev);

            try
            {
                _eventRepository.Delete(ev);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, eventDto);
        }

        protected override void Dispose(bool disposing)
        {
            _scheduleRepository.Dispose();
            _eventRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}