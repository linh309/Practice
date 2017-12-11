using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Db;
using System.Data.Entity;
using UnitOfWork.Practice;
using Practice.Services;
using Unity.Attributes;

namespace UnitOfWork.Practice
{
    public class UnitOfWork
    {
        private Model1 _dbContext;
        public UnitOfWork(Model1 context)
        {
            _dbContext = context;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }

    public class UnitOfWorkGeneric<T> where T : class
    {
        private Model1 _dbContext;
        public UnitOfWorkGeneric()
        {
            //_dbContext = context;        
            var core = new CoreService();
            _dbContext = core.Db;
        }

        public IDbSet<T> DbSet()
        {
            return _dbContext.Set<T>();
        }

        public IDbSet<TEntity> Repo<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}

namespace Practice.Services
{
    public class CoreService: IDisposable
    {
        public static Model1 _db = null;
        public  Model1 Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new Db.Model1();
                }
                return _db;
            }
        }
        public CoreService()
        {
            if (_db == null)
            {
                _db = new Model1();
            }
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class ServiceBase<T> where T : class
    {
        public ServiceBase(UnitOfWorkGeneric<T> unitOfWork)
        {
            unitOfWork = new UnitOfWorkGeneric<T>();
        }
    }

    public interface IBillItemsService
    {
        int AddBillItem();
    }

    public class BillItemService : ServiceBase<BillItems>, IBillItemsService
    {
        private UnitOfWorkGeneric<BillItems> _unitOfWork;
        public BillItemService(UnitOfWorkGeneric<BillItems> unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int AddBillItem()
        {
            _unitOfWork.DbSet().Add(new BillItems
            {
                Name = "Coding: " + DateTime.Now.Ticks.ToString(),
                Price = 19,
                Quantity = 1,
            });

            //return _unitOfWork.DbSet().Count();
            return 1;
        }
    }

    public interface IBillService
    {
        int Add();
    }

    public class BillService : ServiceBase<Bill>, IBillService
    {
        private UnitOfWorkGeneric<Bill> _unitOfWork;
        private IBillItemsService _billItemService;

        public BillService(UnitOfWorkGeneric<Bill> unitOfWork, IBillItemsService billItemService) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _billItemService = billItemService;
        }

        public int Add()
        {
            //var unit1 = new UnitOfWorkGeneric<Bill>(CoreService.Db);
            //var unit2 = new UnitOfWorkGeneric<BillItems>(CoreService.Db);

            //unit1.DbSet().Add(new Bill
            //{
            //    CustomerName = "Linh " + DateTime.Now.Ticks.ToString(),
            //    PayDate = DateTime.Now
            //});
            ////_billItemService.AddBillItem();
            //unit2.DbSet().Add(new BillItems
            //{
            //    Name = "Coding: " + DateTime.Now.Ticks.ToString(),
            //    Price = 19,
            //    Quantity = 1,
            //});

            //return unit1.SaveChanges();


            _unitOfWork.DbSet().Add(new Bill
            {
                CustomerName = "Linh " + DateTime.Now.Ticks.ToString(),
                PayDate = DateTime.Now
            });
            _billItemService.AddBillItem();

            return _unitOfWork.SaveChanges();
        }
    }
}

namespace Practice.Controllers
{
    public class Student
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; }
    }

    public class BaseStudent
    {
        //protected Student Student;
        public BaseStudent(Student st)
        {
            //    st = new Student
            //    {
            //        StudentId = DateTime.Now.Ticks;
            //};
            st = new Student
            {
                StudentId = DateTime.Now.Ticks
            };
            //Student = st;
        }

        //public BaseStudent(int a)
        //{
        //    a = 10;
        //}
    }

    public class BadStudent : BaseStudent
    {
        private Student _badStudent;
        public BadStudent(Student badStudent) : base(badStudent)
        {
            _badStudent = badStudent;
            //_badStudent = base.Student;
        }
    }

    public class HomeController : Controller
    {
        //[Dependency]
        private IBillService _billService { get; set; }
        public HomeController(IBillService billService)
        {
            _billService = billService;
        }

        // GET: Home
        public ActionResult Index()
        {
            _billService.Add();

            //var badStudent = new BadStudent(new Student { StudentId=1});
            //var badStudent = new BadStudent(100);

            return View();
        }

        protected virtual void EndExecute(IAsyncResult asyncResult)
        {
            var a = 10;
            CoreService._db.Dispose();
        }
    }
}