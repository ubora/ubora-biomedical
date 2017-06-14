Feature: Project Creation page
    As a developer
    I want to create the project

Background:
    Given I go to the website "Home/Index"

Scenario: Create a project 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I submit valid project creation form and then project is created
    When I click on the element "a=I have an idea"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I click on the element "a=Sign up"
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "TestFirstName" to the element "#FirstName"
    When I set "TestLastName" to the element "#LastName"
    When I set "email1@email.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "- UBORA"
    When I click on the element "a=Skip profile creation"
    Then I expect the title of the page "Project drafting - UBORA"