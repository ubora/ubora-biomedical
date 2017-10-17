Feature: Work packages Functionality tests
    As a project leader / system administrator
    I want to modify WP1 different workpackages and go through the review process

Background:
    Given I am signed in as user and on first page
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"

Scenario: I make changes in Project overview and check them
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "#ProjectOverview"
    Then I expect the title of the page "Project Overview - UBORA"
    When I select value "Point-of-care diagnosis" from element "#ClinicalNeedTags"
        And I select value "Clinical microbiology" from element "#AreaOfUsageTags"
        And I select value "Mobile-based technology" from element "#PotentialTechnologyTags"
        And I set value "Magnificent other!" to the element "#Gmdn"
        And I click on the element "button=Save changes"
    Then I expect the title of the page "Project Overview - UBORA"
    When I click on the element "a=Project overview"
    Then I expect the element "p=Point-of-care diagnosis" is visible
        And I expect the element "p=Clinical microbiology" is visible
        And I expect the element "p=Mobile-based technology" is visible
        And I expect the element "p=Magnificent other!" is visible
        And I expect the title of the page "Dashboard - UBORA"

Scenario: I click different Workpackages and try to edit them
    When I click on the element "#ProjectOverview"
    Then I expect the element "h1=Project Overview" is visible
    When I click on the element "a=Description of Needs"
    Then I expect the element "h1=Description of Needs" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Needs - UBORA"
    When I click on the element "a=Description of Existing Solutions and Analysis"
    Then I expect the element "h1=Description of Existing Solutions and Analysis" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Existing Solutions and Analysis - UBORA"
    When I click on the element "a=Product Functionality"
    Then I expect the element "h1=Product Functionality" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Functionality - UBORA"
    When I click on the element "a=Product Performance"
    Then I expect the element "h1=Product Performance" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Performance - UBORA"
    When I click on the element "a=Product Usability"
    Then I expect the element "h1=Product Usability" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Usability - UBORA"
    When I click on the element "a=Product Safety"
    Then I expect the element "h1=Product Safety" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Safety - UBORA"
    When I click on the element "a=Patient Population Study"
    Then I expect the element "h1=Patient Population Study" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Patient Population Study - UBORA"
    When I click on the element "a=User Requirement Study"
    Then I expect the element "h1=User Requirement Study" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "User Requirement Study - UBORA"
    When I click on the element "a=Additional Information"
    Then I expect the element "h1=Additional Information" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Additional Information - UBORA"
    When I click on the element "a=Formal review"
    Then I expect the element "h1=Formal review" is visible

Scenario: I Submit project for WP1 review
    When I click on the element "a=Formal review"
        And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
        And I expect the element "b=Status" is visible
        And I expect the element "td=InProcess" is visible

Scenario: System administrator adds Mentor to the project
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"
    When I sign in as administrator
        And I click on the element "h4=Test title"
        And I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "#MentorInviteButton"
    Then I expect the title of the page "Mentors - UBORA"
    When I click on the element "button=Invite mentor"
    Then I expect the element "p=Mentor successfully invited." is visible
    When I click on the element "span=Close"
        And I sign out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Mentor accepts the mentor invitation
    When I sign out
        And I sign in as mentor
        And I click on the element "span=Notifications"
        And I click on the element "button=Accept"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "span=My projects"
    Then I expect the element "h4=Test title" is visible
        And I expect the title of the page "View projects - UBORA"
    When I click on the element "h4=Test title"
    Then I expect the element "a=Repository" is visible
    When I click on the element "a=Members"
    Then I expect the element "a=Test Mentor" is visible
        And I expect the element "title=Project mentor" is visible

Scenario: Project mentor rejects WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
        And I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Reject"
    Then I expect the element "td=Rejected" is visible
    When I click on the element "a=Write a review"
        And I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Reject"
    Then I expect the element "td=Rejected" is visible

Scenario: I submit my rejected WP1 again for formal review
    When I sign out
        And I sign in as user
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
    Then I expect the title of the page "Formal review - UBORA"
    When I click on the element "button=Submit project for review"
    Then I expect the element "td=Rejected" is visible
        And I expect the element "td=InProcess" is visible

Scenario: Project mentor accepts WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
        And I click on the element "a=Write a review"
        And I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "td=Accepted" is visible
        And I expect the element "td=Good project man!" is visible
        And I expect the element "h3=WP2: Conceptual design" is visible

Scenario: I click on WP2 work packages and try to edit them
    When I sign out
        And I sign in as user
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "a=Description of Functions"
    Then I expect the element "h1=Description of Functions" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Functions - UBORA"
    When I click on the element "a=Description of Minimal Requirements for Safety and ISO Compliance"
    Then I expect the element "h1=Description of Minimal Requirements for Safety and ISO Compliance" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Minimal Requirements for Safety and ISO Compliance - UBORA"
    When I click on the element "a=Sketches of Alternate Ideas and Designs"
    Then I expect the element "h1=Sketches of Alternate Ideas and Designs" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Sketches of Alternate Ideas and Designs - UBORA"
    When I click on the element "a=Selection of Best Idea: Reaching the Concept"
    Then I expect the element "h1=Selection of Best Idea: Reaching the Concept" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Selection of Best Idea: Reaching the Concept - UBORA"
    When I click on the element "a=Latest Concept Description"
    Then I expect the element "h1=Latest Concept Description" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Latest Concept Description - UBORA"

Scenario: I Submit project for WP2 review
    When I click on the element ".wp2.formal-review"
        And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"

Scenario: Project mentor rejects WP2 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h4=Test title"
        And I expect the element "a=Work packages" is visible
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
    Then I expect the title of the page "Formal review - UBORA"
    When I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I click on the element "button=Reject"
    Then I expect the element "td=Rejected" is visible

Scenario: I submit my rejected WP2 again for formal review
    When I sign out
        And I sign in as user
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
        And I click on the element "button=Submit project for review"
    Then I expect the element "td=Rejected" is visible
        And I expect the element "td=InProcess" is visible

Scenario: Project mentor accepts WP2 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
        And I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I set value "Great project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "td=Accepted" is visible
        And I expect the element "td=Great project man!" is visible
        And I expect the element "h3=WP3: Design and prototyping" is visible
    When I sign out