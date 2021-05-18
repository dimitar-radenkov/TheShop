namespace TheShop.Database
{
    public interface IDatabaseService
    {
        Article Get(int id);

        Article Add(Article article);
    }
}
