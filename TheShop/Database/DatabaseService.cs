using System;

namespace TheShop.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseDriver databaseDriver;

        public DatabaseService()
        {
            this.databaseDriver = new DatabaseDriver();
        }

        public Article Add(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            return this.databaseDriver.Add(article);
        }

        public Article Get(int id)
        {
            return this.databaseDriver.Get(id);
        }
    }
}
