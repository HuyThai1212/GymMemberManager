using Gym.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace Gym
{
    /// <summary>
    /// Interaction logic for EditMemberWindow.xaml
    /// </summary>
    public partial class EditMemberWindow : Window
    {
        private Member member;
        private readonly GymListManagementtContext _context;

        public EditMemberWindow(Member memberToEdit, GymListManagementtContext sharedContext)
        {
            InitializeComponent();
            _context = sharedContext;
            member = memberToEdit;

            cbMembership.DisplayMemberPath = "PackageName";
            cbMembership.SelectedValuePath = "PackageId";
            cbMembership.SelectedValue = member.MembershipPackageId;
            cbMembership.ItemsSource = _context.MembershipPackages.ToList();

            cbPT.DisplayMemberPath = "Ptname";
            cbPT.SelectedValuePath = "PtpackageId";
            cbPT.SelectedValue = member.PtpackageId ?? 0;
            cbPT.ItemsSource = _context.PersonalTrainerPackages
            .AsNoTracking()
            .ToList()
            .Prepend(new PersonalTrainerPackage { PtpackageId = 0, Ptname = "Chưa đăng ký PT" })
            .ToList();

            txtFullName.Text = member.FullName;
            cbGender.SelectedValue = member.Gender;
            if (member.DateOfBirth.HasValue)
            {
                dpDateOfBirth.SelectedDate = member.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                dpDateOfBirth.SelectedDate = null;
            }

            txtPhone.Text = member.PhoneNumber;
            txtEmail.Text = member.Email;
            dpJoinDate.SelectedDate = member.JoinDate.ToDateTime(TimeOnly.MinValue);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            member.FullName = txtFullName.Text;
            member.Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString();
            member.DateOfBirth = DateOnly.FromDateTime(dpDateOfBirth.SelectedDate ?? DateTime.Now);
            member.PhoneNumber = txtPhone.Text;
            member.Email = txtEmail.Text;
            member.JoinDate = DateOnly.FromDateTime(dpJoinDate.SelectedDate ?? DateTime.Now);
            DialogResult = true;
            member.MembershipPackageId = Convert.ToInt32(cbMembership.SelectedValue);
            int selectedPTId = Convert.ToInt32(cbPT.SelectedValue);
            member.PtpackageId = (selectedPTId == 0) ? null : selectedPTId;
            _context.Members.Update(member);
            _context.SaveChanges();
            Close();
        }
    }
}
