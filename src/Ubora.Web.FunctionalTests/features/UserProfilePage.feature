Feature: User Profile page
    As a developer
    I want to register an account and create my profile

Background:
    Given I go to the website "/Home/Index"

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
    Then I expect the title of the page "- UBORA"
    When I set value "UniversityTest" to the element "#University"
    When I set value "DegreeTest" to the element "#Degree"
    When I set value "FieldTest" to the element "#Field"
    When I select value "Professor" from element "#Role"
    When I set value "Bio Bio Bio, Test Test Test" to the element "#Biography"
    When I set value "SkillsTest" to the element "#Skills"
    When I click on the element "button=Create profile"
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
    Then I expect the input "UniversityTest" of the element "#University" is visible
    Then I expect the input "DegreeTest" of the element "#Degree" is visible
    Then I expect the input "FieldTest" of the element "#Field" is visible
    Then I expect the input "Professor" of the element "#Role" is visible
    Then I expect the input "Bio Bio Bio, Test Test Test" of the element "#Biography" is visible
    Then I expect the input "SkillsTest" of the element "#Skills" is visible
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I change my profile
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    When I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
    When I set value "NameFirst" to the element "#FirstName"
    When I set value "NameLast" to the element "#LastName"
    When I set value "TestUniversity" to the element "#University"
    When I set value "TestDegree" to the element "#Degree"
    When I set value "TestField" to the element "#Field"
    When I select value "Student" from element "#Role"
    When I set value "Test Test Test ,Bio Bio Bio" to the element "#Biography"
    When I set value "TestSkills" to the element "#Skills"
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
    Then I expect the input "NameFirst" of the element "#FirstName" is visible
    Then I expect the input "NameLast" of the element "#LastName" is visible
    Then I expect the input "TestUniversity" of the element "#University" is visible
    Then I expect the input "TestDegree" of the element "#Degree" is visible
    Then I expect the input "TestField" of the element "#Field" is visible
    Then I expect the input "Student" of the element "#Role" is visible
    Then I expect the input "Test Test Test, Bio Bio Bio" of the element "#Biography" is visible
    Then I expect the input "TestSkills" of the element "#Skills" is visible

Scenario: I log out
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    When I click on the element "button=Log out"
    Then I expect the title of the page "Welcome - UBORA"