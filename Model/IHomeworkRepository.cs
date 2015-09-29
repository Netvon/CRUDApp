using System.Collections.Generic;

namespace CRUDApp.Model
{
    public interface IHomeworkRepository
    {
        IList<Homework> GetHomework();
    }
}
