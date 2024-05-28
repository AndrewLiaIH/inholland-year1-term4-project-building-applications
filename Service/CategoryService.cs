using DAL;
using Model;

namespace Service
{
    // This class was written by Andrew Lia
    public class CategoryService
    {
        private CategoryDao categoryDao = new();

        public List<Category> GetAllCategories()
        {
            return categoryDao.GetAllCategories();
        }

        public Category GetCategoryById(int id)
        {
            return categoryDao.GetCategoryById(id);
        }
    }
}
