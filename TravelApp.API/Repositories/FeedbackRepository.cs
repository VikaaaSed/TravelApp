using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IDbContextFactory<FeedbackContext> _context;
        private readonly ILogger<FeedbackRepository> _logger;
        public FeedbackRepository(IDbContextFactory<FeedbackContext> context, ILogger<FeedbackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            if (feedback == null)
            {
                _logger.LogWarning("Попытка создания отзыва с пустым объектом.");
                throw new ArgumentNullException(nameof(feedback));
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {
                await context.Feedbacks.AddAsync(feedback);
                await context.SaveChangesAsync();

                _logger.LogInformation("Отзыв успешно создан. ID: {FeedbackId}", feedback.Id);

                return feedback;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении отзыва: {Feedback}", feedback);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании отзыва.");
                throw;
            }
        }
        public async Task<Feedback?> GetFeedbackAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить отзыва с некорректным Id: {id}", id);
                return null;
            }
            try
            {
                await using var context = await _context.CreateDbContextAsync();
                var feedback = await context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);

                if (feedback == null)
                    _logger.LogInformation("Отзыва с id '{id}' не найден.", id);
                return feedback;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении отзыва по id: {id}", id);
                return null;
            }
        }
        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            if (feedback.Id <= 0)
            {
                _logger.LogWarning("Попытка обновить отзыв с некорректным Id: {id}", feedback.Id);
                return;
            }

            await using var context = await _context.CreateDbContextAsync();
            try
            {

                var oldFeedback = await context.Feedbacks.SingleOrDefaultAsync(f => f.Id == feedback.Id);
                if (oldFeedback == null)
                {
                    _logger.LogWarning("Попытка обновить несуществующий отзыв с id: {id}", feedback.Id);
                    return;
                }
                oldFeedback.NameSender = feedback.NameSender;
                oldFeedback.IsAccepted = feedback.IsAccepted;
                oldFeedback.SenderIpAddress = feedback.SenderIpAddress;
                oldFeedback.Text = feedback.Text;
                oldFeedback.Ball = feedback.Ball;
                oldFeedback.DateTime = feedback.DateTime;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений отзыва с id: {id}", feedback.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных об отзыве по id: {id}", feedback.Id);
                throw;
            }
        }
        public async Task AcceptedFeedbackAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Некорректный ID отзыва: {id}", id);
                return;
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {
                var feedback = await context.Feedbacks.SingleOrDefaultAsync(f => f.Id == id);
                if (feedback == null)
                {
                    _logger.LogInformation("Отзыв с id '{id}' не найден.", id);
                    return;
                }

                feedback.IsAccepted = true;
                await context.SaveChangesAsync();

                _logger.LogInformation("Отзыв с id '{id}' помечен как принятый.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при принятии отзыва с id: {id}", id);
                throw;
            }
        }
        public async Task DeleteFeedbackAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления отзыва с некорректным Id: {id}", id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var city = await context.Feedbacks.SingleOrDefaultAsync(c => c.Id == id);
                if (city == null)
                {
                    _logger.LogWarning("Попытка удаления несуществующего отзыва с id: {id}", id);
                    return;
                }
                context.Feedbacks.Remove(city);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при удалении отзыва с id: {id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении отзыва по id: {id}", id);
                throw;
            }
        }
        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                return await context.Feedbacks.OrderBy(n => n.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка всех отзывов");
                return Enumerable.Empty<Feedback>();
            }
        }
    }
}
