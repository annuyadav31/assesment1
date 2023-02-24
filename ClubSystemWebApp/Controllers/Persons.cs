using ClubSystemWebApp.Data;
using ClubSystemWebApp.Models;
using ClubSystemWebApp.Models.Domain;
using ClubSystemWebApp.Models.Enum;
using ClubSystemWebApp.Models.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClubSystemWebApp.Controllers
{
    public class Persons : Controller
    {
        private readonly ClubSystemDBContext clubSystemDBContext;

        public Persons(ClubSystemDBContext clubSystemDBContext)
        {
            this.clubSystemDBContext = clubSystemDBContext;
        }

        #region "Person"
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var PersonsList = await clubSystemDBContext.Persons.ToListAsync();
            return View(PersonsList);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPersonViewModel addPersonRequest)
        {
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = addPersonRequest.FirstName,
                LastName = addPersonRequest.LastName,
                Phone = addPersonRequest.Phone,
                Email = addPersonRequest.Email,
                Address = addPersonRequest.Address,
            };

           await clubSystemDBContext.Persons.AddAsync(person);
           await clubSystemDBContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> View(Guid Id)
        {
            var Person = await clubSystemDBContext.Persons.FirstOrDefaultAsync(Persons => Persons.Id == Id);

            if (Person != null)
            {
                var viewModel = new UpdatePersonViewModel()
                {
                    Id = Person.Id,
                    FirstName = Person.FirstName,
                    LastName = Person.LastName,
                    Phone = Person.Phone,
                    Email = Person.Email,
                    Address = Person.Address

                };
                return await Task.Run(() => View("View",viewModel));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdatePersonViewModel viewModel)
        {
            var person = await clubSystemDBContext.Persons.FindAsync(viewModel.Id);
            if (person != null)
            {
                person.FirstName = viewModel.FirstName;
                person.LastName = viewModel.LastName;
                person.Phone = viewModel.Phone;
                person.Email = viewModel.Email;
                person.Address = viewModel.Address;

                await clubSystemDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region "Membership"

        [HttpGet]
        public async Task<IActionResult> AddMembership(Guid Id)
        {
            var Person = await clubSystemDBContext.Persons.FirstOrDefaultAsync(Persons => Persons.Id == Id);

            if (Person != null)
            {
                var viewModel = new AddMembershipModel()
                {
                    PersonId = Person.Id,
                };
                return await Task.Run(() => View("AddMembership", viewModel));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMembership(AddMembershipModel addMembershipReq)
        {
            var PersonMembershipList = await clubSystemDBContext.Memberships.Where(x => x.PersonId == addMembershipReq.PersonId).ToListAsync();

            if (PersonMembershipList != null)
            {
                if(PersonMembershipList.Count == 2)
                {
                    ViewBag.Alert = CommonServices.CommonServices.ShowAlert(Alerts.Warning, "Both types of membership are already added for this person");
                    return View("AddMembership");
                }
                else if(PersonMembershipList.Count == 1)
                {
                    if (PersonMembershipList[0].Type == addMembershipReq.Type)
                    {
                        ViewBag.Alert = CommonServices.CommonServices.ShowAlert(Alerts.Warning, "You have already added "+addMembershipReq.Type+" membership for this person.");
                        return View("AddMembership");
                    }
                    else
                    {
                        var personMem = new Membership()
                        {
                            Id = Guid.NewGuid(),
                            Type = addMembershipReq.Type,
                            AccountBalance = addMembershipReq.AccountBalance,
                            PersonId = addMembershipReq.PersonId,
                        };

                        await clubSystemDBContext.Memberships.AddAsync(personMem);
                        await clubSystemDBContext.SaveChangesAsync();
                        var PersonMembershipList1 = await clubSystemDBContext.Memberships.Where(x => x.PersonId == addMembershipReq.PersonId).ToListAsync();
                        return await Task.Run(() => View("ViewMembership", PersonMembershipList1));
                    }
                }
                else
                {
                    var personMem = new Membership()
                    {
                        Id = Guid.NewGuid(),
                        Type = addMembershipReq.Type,
                        AccountBalance = addMembershipReq.AccountBalance,
                        PersonId = addMembershipReq.PersonId,
                    };

                    await clubSystemDBContext.Memberships.AddAsync(personMem);
                    await clubSystemDBContext.SaveChangesAsync();
                    var PersonMembershipList1 = await clubSystemDBContext.Memberships.Where(x => x.PersonId == addMembershipReq.PersonId).ToListAsync();
                    return await Task.Run(() => View("ViewMembership", PersonMembershipList1));

                }
            }
            return View("AddMembership");
        }

        [HttpGet]
        public async Task<IActionResult> ViewMembership(Guid Id)
        {
            var PersonMembershipList = await clubSystemDBContext.Memberships.Where(x => x.PersonId == Id).ToListAsync();

            if (PersonMembershipList != null)
            {
                return View(PersonMembershipList);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllMembership()
        {
            var List = await clubSystemDBContext.Memberships.ToListAsync();

            var FinalList = new List<MembershipWithPersonNameModel>();
            
            foreach (var item in List)
            {
                var person = await clubSystemDBContext.Persons.FirstOrDefaultAsync(x => x.Id == item.PersonId);

                var model = new MembershipWithPersonNameModel()
                {
                    PersonName = person.FirstName + " " + person.LastName,
                    Type = item.Type,
                    AccountBalance = item.AccountBalance
                };
                FinalList.Add(model);
                
            }
            return View(FinalList);
        }

        #endregion
    }
}
