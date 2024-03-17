using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Notifications;
public sealed class EmailNotificationJob
{
    private readonly DataContext _dataContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailNotificationJob> _logger;

    public EmailNotificationJob(DataContext dataContext, IEmailService emailService, ILogger<EmailNotificationJob> logger)
    {
        _dataContext = dataContext;
        _emailService = emailService;
        _logger = logger;
    }

    private async Task<List<Domain.Tasks.Task>> GetAllTasks(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dataContext.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [AutomaticRetry(Attempts = 3)] // Optional: Configure automatic retries for the job
    public async Task Execute()
    {
        try
        {
            // Get all tasks
            var tasks = await GetAllTasks();

            foreach (var task in tasks)
            {
                // Check conditions for sending email (e.g., task.ReminderDateTime)
                if (task.ReminderDateTime.HasValue && task.ReminderDateTime <= DateTime.Now)
                {
                    // Compose email content
                    var emailSubject = $"Task Reminder: {task.Title}";
                    var emailBody = $"Don't forget to complete your task: {task.Description}";

                    // Send email
                    await _emailService.SendEmailAsync(task.User.Email, emailSubject, emailBody);

                    // Update task to mark email as sent
                    task.IsEmailSent = true;

                    // Ensure that the task is being tracked by the context
                    _dataContext.Update(task);

                    // Save changes
                    await _dataContext.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
        }
    }

}