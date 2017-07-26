Feature: User Profile page
    As a user
    I want to register an account and create my profile

Background:
    Given I go to the website "/"

Scenario: I am on Home page 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up an account and create my profile
    When I click on the element "a=Sign in/sign up"
    When I sign up as "gmail@gmail.com" first name "FirstName" last name "LastName"
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
    Then I expect the element "value=FirstName" is visible
    Then I expect the element "value=LastName" is visible
    Then I expect the element "textarea=Bio Bio Bio, Test Test Test" is visible
    Then I expect the element "option=Angola" is visible
    Then I expect the element "value=DegreeTest" is visible
    Then I expect the element "value=FieldTest" is visible
    Then I expect the element "value=UniversityTest" is visible
    Then I expect the element "option=Researcher" is visible
    Then I expect the element "value=InstitutionTest" is visible
    Then I expect the element "value=SkillsTest" is visible
    Then I expect the element "option=Developer" is visible
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
    Then I expect the element "value=NameFirst" is visible
    Then I expect the element "value=NameLast" is visible
    Then I expect the element "textarea=Test Test Test, Bio Bio Bio" is visible
    Then I expect the element "option=Bulgaria" is visible
    Then I expect the element "value=TestDegree" is visible
    Then I expect the element "value=TestField" is visible
    Then I expect the element "value=TestUniversity" is visible
    Then I expect the element "option=Healthcare provider" is visible
    Then I expect the element "value=TestInstitution" is visible
    Then I expect the element "value=TestSkills" is visible
    Then I expect the element "option=Mentor" is visible

Scenario: I log out
    Then I expect the title of the page "Welcome - UBORA"
    When I log out
    Then I expect the title of the page "Welcome - UBORA"