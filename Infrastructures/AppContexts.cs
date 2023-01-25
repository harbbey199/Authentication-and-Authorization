using Credential_Auth.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
{
    public class AppContexts : DbContext
    {
        public AppContexts(DbContextOptions<AppContexts> options) : base(options)
        {

        }
        public DbSet<UserModel> users { get; set; }
    }
}
