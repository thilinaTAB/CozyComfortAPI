using CozyComfortAPI.Model;
using Microsoft.EntityFrameworkCore;
namespace CozyComfortAPI.Data
{
    public class BlanketModelRepo
    {
        private AppDBContext db;
        public BlanketModelRepo(AppDBContext dBContext)
        {
            db = dBContext;
        }

        public bool Save()
        {
            int count = db.SaveChanges();
            if (count > 0)
                return true;
            return false;
        }

        public bool Add(BlanketModel blanketModel)
        {
            if (blanketModel != null)
            {
                bool materialExists = db.Materials.Any(m => m.MaterialID == blanketModel.MaterialID);
                if (!materialExists)
                    return false;

                db.BlanketModels.Add(blanketModel);
                return Save();
            }
            return false;
        }

        public bool Update(BlanketModel blanketModel)
        {
            if (blanketModel != null)
            {
                db.BlanketModels.Update(blanketModel);
                return Save();
            }
            return false;
        }

        public bool Remove(BlanketModel blanketModel)
        {
            if (blanketModel != null)
            {
                db.BlanketModels.Remove(blanketModel);
                return Save();
            }
            return false;
        }

        public List<BlanketModel> GetBlanketModels()
        {
            return db.BlanketModels
                .Include(b => b.Material)
                .ToList();
        }

        public BlanketModel GetBlanketModel(int id)
        {
            return db.BlanketModels.FirstOrDefault(x => x.ModelID == id);
        }
    }
}
