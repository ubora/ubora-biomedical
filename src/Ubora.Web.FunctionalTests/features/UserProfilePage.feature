Feature: User Profile page
    As a user
    I want to register an account and create my profile

Background:
    Given I go to Home page

Scenario: I sign up an account and create my profile
    When I click on the element "span=Log in"
        And I sign up as "user@profile.com"
        And I set value "Bio Bio Bio, Test Test Test" to the element "#Biography"
        And I select value "AGO" from element "#CountryCode"
        And I set value "DegreeTest" to the element "#Degree"
        And I set value "FieldTest" to the element "#Field"
        And I set value "UniversityTest" to the element "#University"
        And I select value "Researcher" from element "#MedicalDevice"
        And I set value "InstitutionTest" to the element "#Institution"
        And I set value "SkillsTest" to the element "#Skills"
        And I select value "Developer" from element "#Role"
        And I click on the element "button=Continue"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I check my created profile
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "#FirstName" to contain text "firstName"
        And I expect the element "#LastName" to contain text "lastName"
        And I expect the element "#Email" to contain text "user@profile.com"
        And I expect the element "#Biography" to contain text "Bio Bio Bio, Test Test Test"
        And I expect the element "#CountryCode" to contain text "AGO"
        And I expect the element "#Degree" to contain text "DegreeTest"
        And I expect the element "#Field" to contain text "FieldTest"
        And I expect the element "#University" to contain text "UniversityTest"
        And I expect the element "#MedicalDevice" to contain text "Researcher"
        And I expect the element "#Institution" to contain text "InstitutionTest"
        And I expect the element "#Skills" to contain text "SkillsTest"
        And I expect the element "#Role" to contain text "Developer"
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I change my profile
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
        And I set value "NameFirst" to the element "#FirstName"
        And I set value "NameLast" to the element "#LastName"
        And I set value "Test Test Test, Bio Bio Bio" to the element "#Biography"
        And I select value "BGR" from element "#CountryCode"
        And I set value "TestDegree" to the element "#Degree"
        And I set value "TestField" to the element "#Field"
        And I set value "TestUniversity" to the element "#University"
        And I select value "Healthcare provider" from element "#MedicalDevice"
        And I set value "TestInstitution" to the element "#Institution"
        And I set value "TestSkills" to the element "#Skills"
        And I select value "Mentor" from element "#Role"
        And I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I check my profile if my changes have been saved
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "#FirstName" to contain text "NameFirst"
        And I expect the element "#LastName" to contain text "NameLast"
        And I expect the element "#Email" to contain text "user@profile.com"
        And I expect the element "#Biography" to contain text "Test Test Test, Bio Bio Bio"
        And I expect the element "#CountryCode" to contain text "BGR"
        And I expect the element "#Degree" to contain text "TestDegree"
        And I expect the element "#Field" to contain text "TestField"
        And I expect the element "#University" to contain text "TestUniversity"
        And I expect the element "#MedicalDevice" to contain text "Healthcare provider"
        And I expect the element "#Institution" to contain text "TestInstitution"
        And I expect the element "#Skills" to contain text "TestSkills"
        And I expect the element "#Role" to contain text "Mentor"  

Scenario: I try to add an empty profile picture
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
        And I click on the element "button=Upload image"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "span=Please select an image to upload first!" is visible

Scenario: I check terms of service
    When I click on the element "span=Profile"
        And I click on the element "a=Terms of Service"
    Then I expect the title of the page "Terms of Service - UBORA"
        And I expect the element "h1=UBORA e-infrastructure Terms of Service and Privacy Policy" is visible
        And I expect the element "a=http://e-infrastructure.ubora-biomedical.org" is visible

Scenario: I sign out
    When I sign out
    Then I expect the title of the page "UBORA"