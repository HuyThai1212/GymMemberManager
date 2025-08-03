using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gym.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Gym
{
    public partial class MainWindow : Window
    {
        private readonly GymListManagementtContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new GymListManagementtContext();

            LoadFilters();
            LoadMembers();
        }

        private void LoadFilters()
        {
            var membershipPackages = _context.MembershipPackages.ToList();
            membershipPackages.Insert(0, new MembershipPackage { PackageId = 0, PackageName = "Tất cả" });
            cmbMembership.ItemsSource = membershipPackages;
            cmbMembership.DisplayMemberPath = "PackageName";
            cmbMembership.SelectedValuePath = "PackageId";
            cmbMembership.SelectedIndex = 0;
            var ptPackages = _context.PersonalTrainerPackages.ToList();
            ptPackages.Insert(0, new PersonalTrainerPackage { PtpackageId = -1, Ptname = "Tất cả" });
            ptPackages.Insert(1, new PersonalTrainerPackage { PtpackageId = 0, Ptname = "Chưa đăng ký PT" });
            cmbPT.ItemsSource = ptPackages;
            cmbPT.DisplayMemberPath = "Ptname";
            cmbPT.SelectedValuePath = "PtpackageId";
            cmbPT.SelectedIndex = 0;
        }

        private void LoadMembers(string? nameFilter = null, int? membershipId = null, int? ptId = null)
        {
            var members = _context.Members
                .Include(m => m.MembershipPackage)
                .Include(m => m.Ptpackage)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                members = members.Where(m => m.FullName.Contains(nameFilter));
            }
            if (membershipId.HasValue && membershipId.Value != -1)
            {
                members = members.Where(m => m.MembershipPackageId == membershipId.Value);
            }
            if (ptId.HasValue)
            {
                if (ptId == 0)
                {
                    members = members.Where(m => m.PtpackageId == null);
                }
                else if (ptId != -1)
                {
                    members = members.Where(m => m.PtpackageId == ptId.Value);
                }
            }

            var memberList = members
                .Select(m => new MemberViewModel
                {
                    MemberId = m.MemberId,
                    FullName = m.FullName,
                    Gender = m.Gender,
                    DateOfBirth = m.DateOfBirth.HasValue ? m.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : null,
                    PhoneNumber = m.PhoneNumber,
                    Email = m.Email,
                    JoinDate = m.JoinDate.ToDateTime(TimeOnly.MinValue),
                    MembershipPackageName = m.MembershipPackage.PackageName,
                    PersonalTrainerPackageName = m.Ptpackage != null ? m.Ptpackage.Ptname : "Chưa đăng ký PT"
                })
                .ToList();

            dgMembers.ItemsSource = memberList;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string nameFilter = txtSearch.Text.Trim();
            int? membershipId = cmbMembership.SelectedValue as int?;
            if (membershipId == 0) membershipId = null;
            int? ptId = cmbPT.SelectedValue as int?;
            LoadMembers(nameFilter, membershipId, ptId);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMemberWindow();
            var result = addWindow.ShowDialog();
            if (result == true)
            {
                var newMember = addWindow.NewMember;

                _context.Members.Add(newMember);
                _context.SaveChanges();
                LoadMembers();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgMembers.SelectedItem is MemberViewModel selected)
            {
                var member = _context.Members
                    .Include(m => m.MembershipPackage)
                    .Include(m => m.Ptpackage)
                    .FirstOrDefault(m => m.MemberId == selected.MemberId);

                if (member != null)
                {
                    var editWindow = new EditMemberWindow(member, _context);
                    var result = editWindow.ShowDialog();
                    if (result == true)
                    {
                        LoadMembers();
                    }
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgMembers.SelectedItem is MemberViewModel selected)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xoá '{selected.FullName}'?", "Xác nhận xoá", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var member = _context.Members.Find(selected.MemberId);
                    if (member != null)
                    {
                        _context.Members.Remove(member);
                        _context.SaveChanges();
                        LoadMembers();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một thành viên để xoá.");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void dgMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

    }
}
