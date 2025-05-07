using EventEase.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EventEase.Service
{
    public class EventService
    {
        private readonly HttpClient _httpClient;

        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET All Events
        public async Task<List<Event>> GetAllEventsAsync()
        {
            var events = await _httpClient.GetFromJsonAsync<List<Event>>("http://localhost:5000/api/EventsApi");
            return events;
        }

        // GET Event by ID
        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventToGet = await _httpClient.GetFromJsonAsync<Event>($"http://localhost:5000/api/EventsApi/{id}");
            return eventToGet;
        }

        // POST Create Event
        public async Task CreateEventAsync(EventCreateDto eventCreateDto)
        {
            await _httpClient.PostAsJsonAsync("http://localhost:5000/api/EventsApi/Create", eventCreateDto);
        }

        // PUT Update Event
        public async Task UpdateEventAsync(int id, EventUpdateDto eventUpdateDto)
        {
            await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/EventsApi/{id}", eventUpdateDto);
        }

        // DELETE Event
        public async Task DeleteEventAsync(int id)
        {
            await _httpClient.DeleteAsync($"http://localhost:5000/api/EventsApi/{id}");
        }
    }
}
