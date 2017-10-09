Feature: Project Creation Dashboard page
    As a user
    I want to create a project and click on all buttons on Project Dashboard page

Background:
    Given I go to Home page

Scenario: Create a project 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up to create a project
    When I click on the element "a=I have an idea"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I sign up as "project@email.com" first name "TestFirstName" last name "TestLastName"
    Then I expect the title of the page "Create a profile - UBORA"
    When I click on the element "button=Continue"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I try to create an empty project
    When I click on the element "a=I have an idea"
        And I click on the element "button=Continue"
    Then I expect the element "span=The Project title field is required." is visible
        And I expect the element "span=The Clinical need field is required." is visible
        And I expect the element "span=The Area field is required." is visible
        And I expect the element "span=The Technology field is required." is visible
        And I expect the title of the page "Project drafting - UBORA"

Scenario: I create a project
    When I click on the element "a=I have an idea"
        And I set value "TestProject" to the element "#Title"
        And I select value "Remote or self-diagnosis" from element "#ClinicalNeedTags"
        And I select value "Pediatric surgery" from element "#AreaOfUsageTags"
        And I select value "Surgical device" from element "#PotentialTechnologyTags"
        And I set value "TestGMDN" to the element "#Gmdn"
        And I click on the element "button=Continue"
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: I click on My projects and open up TestProject
    When I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    
Scenario: On project Dashboard page I click Project overview
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Project overview"
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: On project Dashboard page I click Home
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: On project Dashboard page I click My projects
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"

Scenario: On project Dashboard page I click Work packages
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"

<<<<<<< HEAD
=======
Scenario: I make changes in Project overview and check them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
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

Scenario: On project Dashboard page I click different WP1 Work packages and try to edit them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "#ProjectOverview"
    Then I expect the element "h1=Project Overview" is visible
    When I click on the element "a=Description of Needs"
    Then I expect the element "h1=Description of Needs" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Needs - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Description of Existing Solutions and Analysis"
    Then I expect the element "h1=Description of Existing Solutions and Analysis" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Existing Solutions and Analysis - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Product Functionality"
    Then I expect the element "h1=Product Functionality" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Functionality - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Product Performance"
    Then I expect the element "h1=Product Performance" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Performance - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Product Usability"
    Then I expect the element "h1=Product Usability" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Usability - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Product Safety"
    Then I expect the element "h1=Product Safety" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Product Safety - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Patient Population Study"
    Then I expect the element "h1=Patient Population Study" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Patient Population Study - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=User Requirement Study"
    Then I expect the element "h1=User Requirement Study" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "User Requirement Study - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Additional Information"
    Then I expect the element "h1=Additional Information" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Additional Information - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Formal review"
    Then I expect the element "h1=Formal review" is visible

Scenario: I Submit project for WP1 review
    When I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
        And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
        And I expect the element "b=Status" is visible
        And I expect the element "td=InProcess" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: System administrator adds Mentor to the project
    When I sign in as administrator
        And I click on the element "h4=TestProject"
        And I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "#MentorInviteButton"
    Then I expect the title of the page "Mentors - UBORA"
    When I click on the element "button=Invite mentor"
    Then I expect the element "p=Mentor successfully invited." is visible
    When I click on the element "span=Close"
        And I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Mentor accepts the mentor invitation
    When I sign in as mentor
        And I click on the element "span=Notifications"
        And I click on the element "button=Accept"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "span=My projects"
    Then I expect the element "h4=TestProject" is visible
        And I expect the title of the page "View projects - UBORA"
    When I click on the element "h4=TestProject"
    Then I expect the element "a=Repository" is visible
    When I click on the element "a=Members"
    Then I expect the element "a=Test Mentor" is visible
        And I expect the element "title=Project mentor" is visible

Scenario: Project mentor rejects WP1 formal review
    When I click on the element "h4=TestProject"
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
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I submit my rejected WP1 again for formal review
    When I sign in as "project@email.com" with password "Test12345"
        And I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
    Then I expect the title of the page "Formal review - UBORA"
    When I click on the element "button=Submit project for review"
    Then I expect the element "td=Rejected" is visible
        And I expect the element "td=InProcess" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Project mentor accepts WP1 formal review
    When I sign in as mentor
        And I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element "a=Formal review"
        And I click on the element "a=Write a review"
        And I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "td=Accepted" is visible
        And I expect the element "td=Good project man!" is visible
        And I expect the element "h3=WP2: Conceptual design" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click on WP2 work packages and try to edit them
    When I sign in as "project@email.com" with password "Test12345"
        And I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element "a=Description of Functions"
    Then I expect the element "h1=Description of Functions" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Functions - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Description of Minimal Requirements for Safety and ISO Compliance"
    Then I expect the element "h1=Description of Minimal Requirements for Safety and ISO Compliance" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Description of Minimal Requirements for Safety and ISO Compliance - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Sketches of Alternate Ideas and Designs"
    Then I expect the element "h1=Sketches of Alternate Ideas and Designs" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Sketches of Alternate Ideas and Designs - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Selection of Best Idea: Reaching the Concept"
    Then I expect the element "h1=Selection of Best Idea: Reaching the Concept" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Selection of Best Idea: Reaching the Concept - UBORA"
        And I expect the element ".editor-toolbar" is visible
    When I click on the element "a=Latest Concept Description"
    Then I expect the element "h1=Latest Concept Description" is visible
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Latest Concept Description - UBORA"
        And I expect the element ".editor-toolbar" is visible

Scenario: I Submit project for WP2 review
    When I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
        And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Project mentor rejects WP2 formal review
    When I sign in as mentor
        And I click on the element "h4=TestProject"
        And I expect the element "a=Work packages" is visible
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
    Then I expect the title of the page "Formal review - UBORA"
    When I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I click on the element "button=Reject"
    Then I expect the element "td=Rejected" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I submit my rejected WP2 again for formal review
    When I sign in as "project@email.com" with password "Test12345"
        And I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
        And I click on the element "button=Submit project for review"
    Then I expect the element "td=Rejected" is visible
        And I expect the element "td=InProcess" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Project mentor accepts WP2 formal review
    When I sign in as mentor
        And I click on the element "h4=TestProject"
        And I click on the element "a=Work packages"
        And I click on the element ".wp2.formal-review"
        And I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I set value "Great project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "td=Accepted" is visible
        And I expect the element "td=Great project man!" is visible
        And I expect the element "h3=WP3: Design and prototyping" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

>>>>>>> e3cbe7bf1ba742df4dd3cfe38aa0feafa008c12a
Scenario: I click Repository
    When I click on the element "h4=TestProject"
        And I click on the element "a=Repository"
    Then I expect the title of the page "Repository - UBORA"
        And I expect the element "h1=TestProject" is visible
    When I click on the element "button=Upload new file"
    Then I expect the element "span=Please select a file to upload!" is visible

Scenario: I click Assignments and add an Assignment
    When I click on the element "h4=TestProject"
        And I click on the element "a=Assignments"
    Then I expect the title of the page "Assignments - UBORA"
    When I click on the element "i=add_box"
    Then I expect the title of the page "Assignments - UBORA"
    When I set value "Assignment Title" to the element "#Title"
        And I set value "Assignment Description" to the element "#Description"
        And I click on the element "button=Add Assignment"
    Then I expect the element "a=Assignment Title" is visible
    When I click on the element "a=Assignment Title"
    Then I expect the title of the page "Assignments - UBORA"
        And I expect the element "value=Assignment Description" is visible
    
Scenario: I click Assingments and try to add an empty Assignment
    When I click on the element "h4=TestProject"
        And I click on the element "a=Assignments"
        And I click on the element "i=add_box"
        And I click on the element "button=Add Assignment"
    Then I expect the element "span=The Title field is required." is visible
        And I expect the element "span=The Description field is required." is visible
        And I expect the title of the page "Assignments - UBORA"

Scenario: I click History
    When I click on the element "h4=TestProject"
        And I click on the element "a=History"
    Then I expect the title of the page "History - UBORA"

Scenario: I click Members and try to add a member without email
    When I click on the element "h4=TestProject"
        And I click on the element "a=Members"
        And I click on the element "i=person_add"
    Then I expect the title of the page "Invite member - UBORA"
    When I click on the element "button=Invite member"
    Then I expect the element "span=The Email field is required." is visible

Scenario: I click Members and try to add new member
    When I click on the element "h4=TestProject"
        And I click on the element "a=Members"
        And I click on the element "i=person_add"
        And I set value "emailemail@email.com" to the element "#Email"
    Then I expect the element "value=emailemail@email.com" is visible
    When I click on the element "button=Invite member"
    Then I expect the title of the page "Invite member - UBORA"

Scenario: On Project Dashboard page I click Members and on project owner
    When I click on the element "h4=TestProject"
        And I click on the element "a=Members"
        And I click on the element "a=TestFirstName TestLastName"
    Then I expect the element "p=TestFirstName TestLastName" is visible
        And I expect the title of the page "View profile - UBORA"

Scenario: I click Edit image
    When I click on the element "h4=TestProject"
        And I click on the element "#EditImage"
        And I click on the element "button=Upload image"
    Then I expect the element "span=Please select an image to upload first!" is visible
        And I expect the title of the page "Dashboard - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description
    When I click on the element "h4=TestProject"
    Then I expect the element ".editor-toolbar" is visible
    When I click on the element "#EditProjectDescription"
        And I click on the element ".project-view.full-width"
        And I click on the key "Tab"
        And I click on keys "Welcome to my Project"
        And I click on the element "button=Save changes"
    Then I expect the element "code=Welcome to my Project" is visible
        And I expect the title of the page "Dashboard - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description but Discard it
    When I click on the element "h4=TestProject"
        And I click on the element "#EditProjectDescription"
        And I click on the element "a=Discard"
    Then I expect the element "code=Welcome to my Project" is visible
        And I expect the title of the page "Dashboard - UBORA"