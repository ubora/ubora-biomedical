Feature: Project Creation page
    As a user
    I want to create a project and click on all buttons on Project Dashboard page

Background:
    Given I go to the website "/Home/Index"

Scenario: Create a project 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up to create a project
    When I click on the element "a=I have an idea"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I click on the element "a=Sign up"
    When I click on the element "#IsAgreedToTermsOfService"
    When I set value "TestFirstName" to the element "#FirstName"
    When I set value "TestLastName" to the element "#LastName"
    When I set value "project@email.com" to the element "#Email"
    When I set value "Test12345" to the element "#Password"
    When I set value "Test12345" to the element "#ConfirmPassword"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "Create a profile page - UBORA"
    When I click on the element "a=Skip profile creation"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I create a project
    When I click on the element "a=I have an idea"
    When I set value "TestProject" to the element "#Title"
    When I select value "Child mortality" from element "#ClinicalNeedTags"
    When I select value "Neurology" from element "#AreaOfUsageTags"
    When I select value "Bandages" from element "#PotentialTechnologyTags"
    When I set value "TestGMDN" to the element "#Gmdn"
    When I click on the element "button=Continue"
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

Scenario: On project Dashboard page I click different Work packages and try to edit them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "a=Description of Needs"
    Then I expect the element "h1=Description of Needs" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Description of Needs - UBORA"
    When I click on the element "a=Description of Existing Solutions and Analysis"
    Then I expect the element "h1=Description of Existing Solutions and Analysis" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Description of Existing Solutions and Analysis - UBORA"
    When I click on the element "a=Product Functionality"
    Then I expect the element "h1=Product Functionality" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Product Functionality - UBORA"
    When I click on the element "a=Product Performance"
    Then I expect the element "h1=Product Performance" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Product Performance - UBORA"
    When I click on the element "a=Product Usability"
    Then I expect the element "h1=Product Usability" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Product Usability - UBORA"
    When I click on the element "a=Product Safety"
    Then I expect the element "h1=Product Safety" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Product Safety - UBORA"
    When I click on the element "a=Patient Population Study"
    Then I expect the element "h1=Patient Population Study" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Patient Population Study - UBORA"
    When I click on the element "a=User Requirement Study"
    Then I expect the element "h1=User Requirement Study" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "User Requirement Study - UBORA"
    When I click on the element "a=Additional Information"
    Then I expect the element "h1=Additional Information" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Additional Information - UBORA"

Scenario: On Project Dashboard page I click History
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=History"
    Then I expect the title of the page "History - UBORA"

Scenario: On Project Dashboard page I click Members and try to add new member
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "i=person_add"
    Then I expect the title of the page "Members - UBORA"
    When I set value "emailemail@email.com" to the element "#Email"
    Then I expect the input "emailemail@email.com" of the element "#Email" is correct
    When I click on the element "button=Invite member"
    Then I expect the title of the page "Members - UBORA"

Scenario: On Project Dashboard page I click Members and on project owner
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "a=TestFirstName TestLastName"
    Then I expect the input "TestFirstName TestLastName" of the element ".fullname" is visible
    Then I expect the title of the page "View profile - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element ".project_view.full-width"
    When I click on the key "Tab"
    When I click on keys "Welcome to my Project"
    When I click on the element "button=Save changes"
    Then I expect the input "Welcome to my Project" of the element "p=Welcome to my Project" is visible
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description but Discard it
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Discard"
    Then I expect the input "Welcome to my Project" of the element "p=Welcome to my Project" is visible
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: I go through Device classification
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Work packages"
    Then I expect the title of the page "Work packages - UBORA"
    When I click on the element "a=Device classification"
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "a=Go to device classification"
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "button=Is your device NON INVASIVE?"
    When I click on the element "button=Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
    When I click on the element "button=May it be connected to an active medical device in class IIa or a higher class?"
    When I click on the element "button=Is your device ACTIVE?"
    When I click on the element "button=Is it therapeutic?"
    When I click on the element "button=Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?"
    When I click on the element "button=Does your device incorporate a MEDICAL PRODUCT with an ancillary function?"
    When I click on the element "button=Is it used for contraception or prevention of the transmission of sexually transmitted diseases?"
    When I click on the element "button=Is it implantable or long term invasive?"
    When I click on the element "button=Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?"
    When I click on the element "button=Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses?"
    When I click on the element "button=Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?"
    When I click on the element "button=Is it manufactured utilising tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?"
    When I click on the element "button=Does it incorporate or contain nanomaterials?"
    When I click on the element "button=Does it present a high or medium potential for internal exposure?"
    When I click on the element "button=Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?"
    When I click on the element "button=Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?"
    When I click on the element "button=Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?"
    When I click on the element "button=Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?"
    When I click on the element "button=Is it an active therapeutic devices with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?"
    Then I expect the element "h1=Your device classification is: III" is visible
    Then I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Retake questionnaire?"
    Then I expect the title of the page "Device classification - UBORA"
    When I go back to last page
    Then I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Go back to project page"
    Then I expect the title of the page "Dashboard - UBORA"