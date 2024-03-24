using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iBudget.Framework;

namespace iBudget.DAO.Entities;

public class LoginLogModel
{
    public LoginLogModel()
    {
        DateTime = DateTime.Now;
        Status = LoginLogStatus.None;
    }

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Key]
    public int LoginLogId { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string RemoteIpAddress { get; set; }
    public DateTime DateTime { get; set; }
    public LoginLogStatus Status { get; set; }
}