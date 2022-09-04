namespace LearnEF.Database.Conventions
{
    public abstract class BaseTable
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
