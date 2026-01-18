using Microsoft.AspNetCore.Identity;
using ProjeAyuDeneme.Models;

namespace ProjeAyuDeneme.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CurrentRole { get; set; } = string.Empty;
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public string SelectedRole { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Birim { get; set; } = string.Empty;
        public string Pozisyon { get; set; } = string.Empty;
    }
}