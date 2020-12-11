using System;

namespace GymRat.Models
{
    public class Exercise
    {
        public Guid ExerciseId { get; set; }
        public Guid SessionId { get; set; }
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public TimeSpan Duration { get; set; }
    }
}