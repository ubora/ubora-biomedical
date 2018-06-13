Feature: Project Creation Dashboard page
    As a user
    I want to create a project and click on all buttons on Project Dashboard page

Background:
    Given I go to Home page

Scenario: I sign in to create a project
    When I sign in as user
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I try to create an empty project
    When I click on the element "a=Create project"
        And I click on the element "button=Create project"
    Then I expect the element "span=The Project title field is required." is visible
        And I expect the element "span=The Clinical need field is required." is visible
        And I expect the element "span=The Area field is required." is visible
        And I expect the element "span=The Technology field is required." is visible
        And I expect the title of the page "Project drafting - UBORA"

Scenario: I create a project
    When I click on the element "a=Create project"
        And I set value "TestProject" to the element "#Title"
        And I select value "Remote or self-diagnosis" from element "#ClinicalNeedTags"
        And I select value "Pediatric surgery" from element "#AreaOfUsageTags"
        And I select value "Surgical device" from element "#PotentialTechnologyTags"
        And I set value "TestGMDN" to the element "#Gmdn"
        And I click on the element "button=Create project"
    Then I expect the title of the page "Overview - UBORA"

Scenario: I click on My projects and open up TestProject
    When I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"
    When I click on the element "h5=TestProject"
    Then I expect the title of the page "Overview - UBORA"

Scenario: I click Project overview
    When I click on the element "h5=TestProject"
    Then I expect the title of the page "Overview - UBORA"
    When I click on the element "a=Overview"
    Then I expect the title of the page "Overview - UBORA"

Scenario: I click Home
    When I click on the element "h5=TestProject"
    Then I expect the title of the page "Overview - UBORA"
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click My projects
    When I click on the element "h5=TestProject"
    Then I expect the title of the page "Overview - UBORA"
    When I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"

Scenario: I click Work packages
    When I click on the element "h5=TestProject"
    Then I expect the title of the page "Overview - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Design planning - UBORA"

Scenario: I click Repository
    When I click on the element "h5=TestProject"
        And I click on the element "a=Repository"
    Then I expect the title of the page "Repository - UBORA"
    When I click on the element "button=Upload new file"
    Then I expect the element "span=Please select a file to upload!" is visible
        And I expect the element "span=Please select a file to upload!" is visible

Scenario: I click Assignments and add an Assignment
    When I click on the element "h5=TestProject"
        And I click on the element "a=Assignments"
    Then I expect the title of the page "Assignments - UBORA"
    When I click on the element "span=Add assignment"
    Then I expect the title of the page "Add assignment - UBORA"
    When I set value "Assignment Title" to the element "#Title"
        And I set value "Assignment Description" to the element "#Description"
        And I click on the element "button=Add assignment"
    Then I expect the element "a=Assignment Title" is visible
    When I click on the element "a=Assignment Title"
    Then I expect the title of the page "Edit assignment - UBORA"
        And I expect the element "textarea=Assignment Description" is visible

Scenario: I click Assignments and try to add an empty Assignment
    When I click on the element "h5=TestProject"
        And I click on the element "a=Assignments"
        And I click on the element "span=Add assignment"
        And I click on the element "button=Add assignment"
    Then I expect the element "span=The Title field is required." is visible
        And I expect the element "span=The Description field is required." is visible

Scenario: I click Assignments and click Discard changes in new assignment
    When I click on the element "h5=TestProject"
        And I click on the element "a=Assignments"
        And I click on the element "span=Add assignment"
        And I click on the element "a=Discard"
    Then I expect the title of the page "Assignments - UBORA"
        And I expect the element "h1=Assignments" is visible

Scenario: I click Assignments and click Discard changes in Assignment
    When I click on the element "h5=TestProject"
        And I click on the element "a=Assignments"
        And I click on the element "a=Assignment Title"
    Then I expect the element "h1=Assignment" is visible
    When I click on the element "a=Discard"
    Then I expect the element "a=Assignment Title" is visible
        And I expect the title of the page "Assignments - UBORA"

Scenario: I click History
    When I click on the element "h5=TestProject"
        And I click on the element "a=History"
    Then I expect the title of the page "History - UBORA"

Scenario: I click Members and try to add a member without email
    When I click on the element "h5=TestProject"
        And I click on the element "a=Members"
        And I click on the element "i=person_add"
    Then I expect the title of the page "Invite member - UBORA"
    When I click on the element "button=Invite member"
    Then I expect the element "span=The Email field is required." is visible

Scenario: I click Members and try to add new member
    When I click on the element "h5=TestProject"
        And I click on the element "a=Members"
        And I click on the element "i=person_add"
        And I set value "emailemail@email.com" to the element "#Email"
        And I click on the element "button=Invite member"
    Then I expect the title of the page "Invite member - UBORA"

Scenario: I click Members and on project owner
    When I click on the element "h5=TestProject"
        And I click on the element "a=Members"
        And I click on the element "a=Test User"
    Then I expect the element "h2=Test User" is visible
        And I expect the title of the page "View profile - UBORA"

Scenario: I click Edit image
    When I click on the element "h5=TestProject"
        And I click on the element "#EditImage"
        And I click on the element "button=Upload image"
    Then I expect the element "span=Please select an image to upload first!" is visible
        And I expect the title of the page "Dashboard - UBORA"

Scenario: I click Edit Project Description
    When I click on the element "h5=TestProject"
    Then I expect the element "h2=Medical tags" is visible
    When I click on the element "#EditDescription"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Describe in few words the device technology/intended use/and intended clinical benefits of the device. Describe who are the intended users. Describe if there are some limitation about the use of the device (for example need of continuous power supply) and if there are contraindications." is visible
    When I click on the key "Tab"
        And I click on keys "Welcome to my Project"
        And I click on the element "button=Save changes"
    Then I expect the element "div=Welcome to my Project" is visible
        And I expect the title of the page "Overview - UBORA"

Scenario: I click Edit Project Description but Discard it
    When I click on the element "h5=TestProject"
        And I click on the element "#EditDescription"
        And I click on the element "a=Discard"
    Then I expect the element "div=Welcome to my Project" is visible
        And I expect the title of the page "Overview - UBORA"
