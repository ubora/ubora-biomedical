Feature: User Profile changes
    As a user
    I want to register an account and change my password

Background:
    Given I go to the website "/Home/Index"

Scenario: Get the title of Ubora
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I register an account
    When I click on the element "a=Sign in/sign up"
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"
    When I set value "Change" to the element "#FirstName"
    When I set value "Password" to the element "#LastName"
    When I set value "change@password.com" to the element "#Email"
    When I set value "Test12345" to the element "#Password"
    When I set value "Test12345" to the element "#ConfirmPassword"
    When I click on the element "#IsAgreedToTermsOfService"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "Create a profile page - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=TestFirstName TestLastName" is visible

Scenario: I log out
    When I click on the element "span=Menu"
    When I click on the element "button=Log out"
    Then I expect the title of the page "Welcome - UBORA"
    Then I expect the element "a=Sign in/sign up" is visible