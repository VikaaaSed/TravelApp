﻿using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<Feedback> CreateAsync(Feedback feedback);
    }
}
