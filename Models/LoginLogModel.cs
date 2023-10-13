using iBudget.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class LoginLogModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int LoginLogId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Hostname { get; set; } = string.Empty;
    public string RemoteIpAddress { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
    public LoginLogStatus Status { get; set; } = LoginLogStatus.None;
}
