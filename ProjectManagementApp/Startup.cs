using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Model;

namespace ProjectManagementApp {
    public class Startup {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration) { 
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) { 
            services.AddDbContext<TaskContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();
        }
    }
}
