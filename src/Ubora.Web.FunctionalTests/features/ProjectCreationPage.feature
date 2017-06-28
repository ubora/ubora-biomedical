Feature: Project Creation page
    As a developer
    I want to create the project

Background:
    Given I go to the website "/Home/Index"

Scenario: Create a project 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up to create a project
    When I click on the element "a=I have an idea"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I click on the element "a=Sign up"
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I set "TestLastName" to the element "#LastName"
    When I set "project@email.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "- UBORA"
    When I click on the element "a=Skip profile creation"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I create a project
    When I click on the element "a=I have an idea"
    When I set "TestProject" to the element "#Title"
    When I select "Child mortality" from element "#ClinicalNeed"
    When I select "Neurology" from element "#AreaOfUsage"
    When I select "Bandages" from element "#PotentialTechnology"
    When I set "TestGMDN" to the element "#Keywords"
    When I click on the element "button=Continue"
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: I click on My projects and open up TestProject
    When I click on the element "i=folder"
    Then I expect the title of the page "- UBORA"
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
    Then I expect the title of the page "- UBORA"

Scenario: On project Dashboard page I click Workpackages
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Workpackages"
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: On project Dashboard page I click different Work packages and try to edit them
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Workpackages"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Description Of Need"
    Then I expect the input "Description Of Need" of the element "h1=Description Of Need" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Description Of Existing Solutions And Analysis"
    Then I expect the input "Description Of Existing Solutions And Analysis" of the element "h1=Description Of Existing Solutions And Analysis" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Product Functionality"
    Then I expect the input "Product Functionality" of the element "h1=Product Functionality" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Product Performance"
    Then I expect the input "Product Performance" of the element "h1=Product Performance" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Product Usability"
    Then I expect the input "Product Usability" of the element "h1=Product Usability" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Product Safety"
    Then I expect the input "Product Safety" of the element "h1=Product Safety" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Patient Population Study"
    Then I expect the input "Patient Population Study" of the element "h1=Patient Population Study" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=User Requirement Study"
    Then I expect the input "User Requirement Study" of the element "h1=User Requirement Study" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"
    When I click on the element "a=Additional Information"
    Then I expect the input "Additional Information" of the element "h1=Additional Information" is visible
    When I click on the element ".step_edit"
    Then I expect the title of the page "Workpackages - UBORA"

Scenario: On Project Dashboard page I click Activity
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Activity"
    Then I expect the title of the page "History - UBORA"

Scenario: On Project Dashboard page I click Members and try to add new member
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "i=person_add"
    Then I expect the title of the page "Members - UBORA"
    When I set "emailemail@email.com" to the element "#Email"
    Then I expect the input "emailemail@email.com" of the element "#Email" is correct
    When I click on the element "button=Invite"
    Then I expect the title of the page "Members - UBORA"

Scenario: On Project Dashboard page I click Members and on project owner
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "i=supervisor_account"
    Then I expect the input "TestFirstName" of the element "#FirstName" is correct
    Then I expect the input "TestLastName" of the element "#LastName" is correct
    Then I expect the title of the page "View profile - UBORA"

Scenario: On Project Dashboard page I click Edit Project Description
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "i=mode_edit"
    Then I expect the title of the page "- UBORA"
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
    Then I expect the title of the page "- UBORA"
    When I click on the element "a=Discard"
    Then I expect the input "Welcome to my Project" of the element "p=Welcome to my Project" is visible
    Then I expect the title of the page "Dashboard - UBORA"

Scenario: I go through Device classification
    When I click on the element "h4=TestProject"
    Then I expect the title of the page "Dashboard - UBORA"
    When I click on the element "a=Go to device classification"
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "button=Is your device NON INVASIVE?"
    When I click on the element "button=Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
    When I click on the element "button=May it be connected to an active medical device in class IIa or a higher class?"
    When I click on the element "button=Is your device ACTIVE?"
    When I click on the element "button=Is it therapeutic?"
    When I click on the element "button=Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?"
    When I click on the element "button=Does your device incorporate a medicinal product with an ancillary function?"
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
    When I click on the element "a=Please retake the questionnaire"
    Then I expect the title of the page "Device classification - UBORA"
    When I go Back to last page
    Then I expect the title of the page "- UBORA"
    When I click on the element "a=go back to project page."
    Then I expect the input "III" of the element "b=III" is visible
    Then I expect the title of the page "Dashboard - UBORA"