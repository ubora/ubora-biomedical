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
    And I select value "Child mortality" from element "#ClinicalNeedTags"
    And I select value "Neurology" from element "#AreaOfUsageTags"
    And I select value "Bandages" from element "#PotentialTechnologyTags"
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

Scenario: I make changes in Design planning and check them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "a=Design planning"
    Then I expect the title of the page "Design planning - UBORA"
    When I select value "Movement impairment" from element "#ClinicalNeedTags"
    And I select value "Cardiology" from element "#AreaOfUsageTags"
    And I select value "Wheelchairs" from element "#PotentialTechnologyTags"
    And I set value "Magnificent other!" to the element "#Gmdn"
    And I click on the element "button=Save changes"
    Then I expect the title of the page "Design planning - UBORA"
    When I click on the element "a=Project overview"
    Then I expect the element "p=Movement impairment" is visible
    And I expect the element "p=Cardiology" is visible
    And I expect the element "p=Wheelchairs" is visible
    And I expect the element "p=Magnificent other!" is visible
    And I expect the title of the page "Dashboard - UBORA"

Scenario: On project Dashboard page I click different WP1 Work packages and try to edit them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "a=Design planning"
    Then I expect the element "h1=Design planning" is visible
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
    When I click on the element "h4=TestProject"
    And I click on the element "a=Work packages"
    And I click on the element "a=Formal review"
    And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
    And I expect the element "b=Status" is visible
    And I expect the element "td=InProcess" is visible
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I review project WP1 as system administrator and reject it
    When I sign in as administrator
    And I click on the element "h4=TestProject"
    And I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "button=Assign me as project mentor"
    Then I expect the element "a=Work packages" is visible
    When I click on the element "a=Work packages"
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

Scenario: I review WP1 as system administrator and accept it
    When I sign in as administrator
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
    When I click on the element "h4=TestProject"
    And I click on the element "a=Work packages"
    And I click on the element ".wp2.formal-review"
    And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
    When I log out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: System administrator rejects WP2 formal review
    When I sign in as administrator
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

Scenario: I review WP2 as system administrator and accept it
    When I sign in as administrator
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

Scenario: I click Repository
    When I sign in as "project@email.com" with password "Test12345"
    And I click on the element "h4=TestProject"
    When I click on the element "a=Repository"
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
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "button=Invite member"
    Then I expect the element "span=The Email field is required." is visible

Scenario: I click Members and try to add new member
    When I click on the element "h4=TestProject"
    And I click on the element "a=Members"
    And I click on the element "i=person_add"
    And I set value "emailemail@email.com" to the element "#Email"
    Then I expect the element "value=emailemail@email.com" is visible
    When I click on the element "button=Invite member"
    Then I expect the title of the page "Members - UBORA"

Scenario: On Project Dashboard page I click Members and on project owner
    When I click on the element "h4=TestProject"
    And I click on the element "a=Members"
    And I click on the element "a=TestFirstName TestLastName"
    Then I expect the element "p=TestFirstName TestLastName" is visible
    And I expect the title of the page "View profile - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description
    When I click on the element "h4=TestProject"
    And I click on the element "i=mode_edit"
    And I click on the element ".project-view.full-width"
    And I click on the key "Tab"
    And I click on keys "Welcome to my Project"
    And I click on the element "button=Save changes"
    Then I expect the element "code=Welcome to my Project" is visible
    And I expect the title of the page "Dashboard - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description but Discard it
    When I click on the element "h4=TestProject"
    And I click on the element "i=mode_edit"
    And I click on the element "a=Discard"
    Then I expect the element "code=Welcome to my Project" is visible
    And I expect the title of the page "Dashboard - UBORA"

Scenario: I go through Device classification
    When I click on the element "h4=TestProject"
    And I click on the element "a=Work packages"
    And I click on the element "a=Device classification"
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "a=Go to device classification"
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "button=Is your device NON INVASIVE?"
    And I click on the element "button=Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
    And I click on the element "button=May it be connected to an active medical device in class IIa or a higher class?"
    And I click on the element "button=Is your device ACTIVE?"
    And I click on the element "button=Is it therapeutic?"
    And I click on the element "button=Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?"
    And I click on the element "button=Does your device incorporate a MEDICAL PRODUCT with an ancillary function?"
    And I click on the element "button=Is it used for contraception or prevention of the transmission of sexually transmitted diseases?"
    And I click on the element "button=Is it implantable or long term invasive?"
    And I click on the element "button=Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?"
    And I click on the element "button=Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses?"
    And I click on the element "button=Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?"
    And I click on the element "button=Is it manufactured utilising tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?"
    And I click on the element "button=Does it incorporate or contain nanomaterials?"
    And I click on the element "button=Does it present a high or medium potential for internal exposure?"
    And I click on the element "button=Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?"
    And I click on the element "button=Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?"
    And I click on the element "button=Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?"
    And I click on the element "button=Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?"
    And I click on the element "button=Is it an active therapeutic devices with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?"
    Then I expect the element "h1=Your device classification is: III" is visible
    And I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Retake questionnaire?"
    Then I expect the title of the page "Device classification - UBORA"
    When I go back to last page
    Then I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Go back to project page"
    Then I expect the title of the page "Dashboard - UBORA"