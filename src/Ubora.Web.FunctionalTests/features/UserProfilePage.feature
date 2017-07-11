Feature: User Profile page
    As a user
    I want to register an account and create my profile

Background:
    Given I go to the website "/"

Scenario: Go to Home page 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up an account and create my profile
    When I click on the element "a=Sign in/sign up"
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"
    When I click on the element "#IsAgreedToTermsOfService"
    When I set value "FirstName" to the element "#FirstName"
    When I set value "LastName" to the element "#LastName"
    When I set value "gmail@gmail.com" to the element "#Email"
    When I set value "Test12345" to the element "#Password"
    When I set value "Test12345" to the element "#ConfirmPassword"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "Create a profile - UBORA"
    When I set value "Bio Bio Bio, Test Test Test" to the element "#Biography"
    When I select value "AGO" from element "#CountryCode"
    When I set value "DegreeTest" to the element "#Degree"
    When I set value "FieldTest" to the element "#Field"
    When I set value "UniversityTest" to the element "#University"
    When I select value "Researcher" from element "#MedicalDevice"
    When I set value "InstitutionTest" to the element "#Institution"
    When I set value "SkillsTest" to the element "#Skills"
    When I select value "Developer" from element "#Role"
    When I click on the element "button=Continue"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I check my created profile
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    When I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
    Then I expect the input "FirstName" of the element "#FirstName" is visible
    Then I expect the input "LastName" of the element "#LastName" is visible
    Then I expect the input "Bio Bio Bio, Test Test Test" of the element "#Biography" is visible
    Then I expect the input "AGO" of the element "#CountryCode" is visible
    Then I expect the input "DegreeTest" of the element "#Degree" is visible
    Then I expect the input "FieldTest" of the element "#Field" is visible
    Then I expect the input "UniversityTest" of the element "#University" is visible
    Then I expect the input "Researcher" of the element "#MedicalDevice" is visible
    Then I expect the input "InstitutionTest" of the element "#Institution" is visible
    Then I expect the input "SkillsTest" of the element "#Skills" is visible
    Then I expect the input "Developer" of the element "#Role" is visible
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I change my profile and check if my changes have been saved
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    When I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
    When I set value "NameFirst" to the element "#FirstName"
    When I set value "NameLast" to the element "#LastName"
    When I set value "Test Test Test, Bio Bio Bio" to the element "#Biography"
    When I select value "BGR" from element "#CountryCode"
    When I set value "TestDegree" to the element "#Degree"
    When I set value "TestField" to the element "#Field"
    When I set value "TestUniversity" to the element "#University"
    When I select value "Healthcare provider" from element "#MedicalDevice"
    When I set value "TestInstitution" to the element "#Institution"
    When I set value "TestSkills" to the element "#Skills"
    When I select value "Mentor" from element "#Role"
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
    Then I expect the input "NameFirst" of the element "#FirstName" is visible
    Then I expect the input "NameLast" of the element "#LastName" is visible
    Then I expect the input "Test Test Test, Bio Bio Bio" of the element "#Biography" is visible
    Then I expect the input "BGR" of the element "#CountryCode" is visible
    Then I expect the input "TestDegree" of the element "#Degree" is visible
    Then I expect the input "TestField" of the element "#Field" is visible
    Then I expect the input "TestUniversity" of the element "#University" is visible
    Then I expect the input "Healthcare provider" of the element "#MedicalDevice" is visible
    Then I expect the input "TestInstitution" of the element "#Institution" is visible
    Then I expect the input "TestSkills" of the element "#Skills" is visible
    Then I expect the input "Mentor" of the element "#Role" is visible

Scenario: I log out
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    When I click on the element "button=Log out"
    Then I expect the title of the page "Welcome - UBORA"