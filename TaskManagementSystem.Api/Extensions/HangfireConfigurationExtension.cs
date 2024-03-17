using Hangfire;
using TaskManagementSystem.Application.Notifications;

namespace TaskManagementSystem.Api.Extensions;
internal static class HangfireConfigurationExtension
{
    public static void AddHangfire(this IServiceCollection services)
    {
        // Add Hangfire services
        services.AddHangfireServer();
    }

    public static void UseHangfire(this IApplicationBuilder app)
    {
        // Configure Hangfire dashboard
        app.UseHangfireDashboard();

        // Schedule the EmailNotificationJob to run
        RecurringJob.AddOrUpdate<EmailNotificationJob>(
            "SendTaskReminders",
            job => job.Execute(),
            "0 */15 * * * ?");

        // Every 15 mins => 0 */ 15 * ** ?
        // Every 2 secs => 0/2 * * ? * *
    }
}
