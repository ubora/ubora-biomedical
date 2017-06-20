Feature: UserProfilePage
    As a developer
    I want to register an account and create my profile

Background:
    Given I go to the website "/Home/Index"
    Given I click the element "a=Sign in/sign up"
    Given I click the element ".login-button-image"
    Given I click the element ".button.secondary-button.full-width"

Scenario: Go to SignUp page 
    Then I expect the title of the page "Sign up - UBORA"

Scenario: I sign up an account and edit my profile
    When I click on the element "#IsAgreedToTermsOfService"
    When I set "FirstName" to the element "#FirstName"
    When I set "LastName" to the element "#LastName"
    When I set "gmail@gmail.com" to the element "#Email"
    When I set "Test12345" to the element "#Password"
    When I set "Test12345" to the element "#ConfirmPassword"
    When I click on the element "/html/body/main/section/form/button"
    Then I expect the title of the page "- UBORA"