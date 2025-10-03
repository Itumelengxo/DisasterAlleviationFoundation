using System.Collections.Concurrent;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    public static class InMemoryStore
    {
        // Thread-safe collections
        public static ConcurrentDictionary<string, UserModel> Users { get; } = new ConcurrentDictionary<string, UserModel>(StringComparer.OrdinalIgnoreCase);
        public static ConcurrentDictionary<int, Incident> Incidents { get; } = new ConcurrentDictionary<int, Incident>();
        public static ConcurrentDictionary<int, Donation> Donations { get; } = new ConcurrentDictionary<int, Donation>();
        public static ConcurrentDictionary<int, Volunteer> Volunteers { get; } = new ConcurrentDictionary<int, Volunteer>();
        public static ConcurrentDictionary<int, VolunteerTask> VolunteerTasks { get; } = new ConcurrentDictionary<int, VolunteerTask>();

        // Simple incremental id generators
        private static int _incidentId = 0;
        private static int _donationId = 0;
        private static int _volunteerId = 0;
        private static int _taskId = 0;

        public static int NextIncidentId() => Interlocked.Increment(ref _incidentId);
        public static int NextDonationId() => Interlocked.Increment(ref _donationId);
        public static int NextVolunteerId() => Interlocked.Increment(ref _volunteerId);
        public static int NextTaskId() => Interlocked.Increment(ref _taskId);
    }
}
