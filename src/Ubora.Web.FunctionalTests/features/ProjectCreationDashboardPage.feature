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
    When I sign up as "project@email.com"
    Then I expect the title of the page "Create a profile - UBORA"
    When I click on the element "button=Continue"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I try to create an empty project
    When I click on the element "a=I have an idea"
        And I click on the element "button=Continue"
    Then I expect the element "span=The Project title field is required." is visible
        And I expect the element "span=The Clinical need field is required." is visible
        And I expect the element "span=The Area field is required." is visible
        And I expect the element "span=The Technosigny field is required." is visible
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
        And I click on the element "a=firstName lastName"
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
