namespace Movies.Domain
{
	public abstract class Entity<TKey>
	{
		public virtual TKey Id { get; set; }
	}
}