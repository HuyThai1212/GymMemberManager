using Gym.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gym
{
    public partial class AddMemberWindow : Window
    {
        public Member NewMember { get; private set; }

        private readonly GymListManagementtContext _context;

        public AddMemberWindow()
        {
            InitializeComponent();
            _context = new GymListManagementtContext();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            var membershipPackages = _context.MembershipPackages.AsNoTracking().ToList();
            var ptPackages = _context.PersonalTrainerPackages.AsNoTracking().ToList();

            ptPackages.Insert(0, new PersonalTrainerPackage
            {
                PtpackageId = 0,
                Ptname = "Không đăng ký PT"
            });
            cbMembership.ItemsSource = membershipPackages;
            cbMembership.DisplayMemberPath = "PackageName";
            cbMembership.SelectedValuePath = "PackageId";

            cbPT.ItemsSource = ptPackages;
            cbPT.DisplayMemberPath = "Ptname";
            cbPT.SelectedValuePath = "PtpackageId";
            cbPT.SelectedIndex = 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (cbMembership.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn gói tập và gói PT.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int? ptId = null;
            if (cbPT.SelectedValue is int selectedPtId && selectedPtId != 0)
            {
                ptId = selectedPtId;
            }

            NewMember = new Member
            {
                FullName = txtFullName.Text,
                Gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                DateOfBirth = DateOnly.FromDateTime(dpDateOfBirth.SelectedDate ?? DateTime.Now),
                PhoneNumber = txtPhone.Text,
                Email = txtEmail.Text,
                JoinDate = DateOnly.FromDateTime(dpJoinDate.SelectedDate ?? DateTime.Now),
                MembershipPackageId = Convert.ToInt32(cbMembership.SelectedValue),
                PtpackageId = ptId
            };

            DialogResult = true;
            Close();
        }
    }
}

