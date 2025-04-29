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
        public async Task UpdateCityAsync(Feedback feedback)
        {
            if (feedback.Id <= 0)
            {
                _logger.LogWarning("Попытка обновить отзыв с некорректным Id: {id}", feedback.Id);
                return;
            }

            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
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
                await transaction.CommitAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений отзыва с id: {id}", feedback.Id);
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных об отзыве по id: {id}", feedback.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
