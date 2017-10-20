using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Commands;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class ConceptualDesignController : ProjectController
    {
        public ConceptualDesignController()
        {

        }


        public IActionResult AddCandidate()
        {
            var model = new AddCandidateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCandidate(AddCandidateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }


            ExecuteUserProjectCommand(new AddCandidateCommand
            {
                Id = Guid.NewGuid(),
                Title = model.Name,
                Description = model.Description,
            });

            if (!ModelState.IsValid)
            {
                return AddCandidate();
            }

            return RedirectToAction(nameof(AddCandidate));
        }
    }
}
