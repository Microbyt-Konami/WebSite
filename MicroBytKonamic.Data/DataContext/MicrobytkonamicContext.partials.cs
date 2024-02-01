using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Data.DataContext
{
    public partial class MicrobytkonamicContext
    {
        private readonly IConfiguration _configuration;

        public MicrobytkonamicContext(DbContextOptions<MicrobytkonamicContext> options, IConfiguration configuration)
        : this(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseMySql("server=localhost;user=microbytkonamic;password=&M0z00s$;database=microbytkonamic", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));
        => optionsBuilder.UseMySql(_configuration.GetConnectionString("MicroBytKonamic"), ServerVersion.Parse("8.0.35-mysql"));
    }
}
