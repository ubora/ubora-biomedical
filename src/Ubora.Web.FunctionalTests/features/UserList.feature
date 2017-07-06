Feature: User list page
    As a user
    I want to click on all buttons on Members page

Background:
    Given I go to the website "/Home/Index"
    Given I clicked on the element "a=View members"

Scenario: Get the title of Ubora 
    Then I expect the title of the page "View members - UBORA"

Scenario: I click a Ubora logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click a Sign in/sign up button
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I sign up and check my profile on Members page
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"
    When I set value "TestName" to the element "#FirstName"
    When I set value "TestLastName" to the element "#LastName"
    When I set value "emailemail@email.com" to the element "#Email"
    When I set value "Test12345" to the element "#Password"
    When I set value "Test12345" to the element "#ConfirmPassword"
    When I click on the element "#IsAgreedToTermsOfService"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "- UBORA"
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "a=View members"
    Then I expect the title of the page "View members - UBORA"
    When I click on the element "a=TestName TestLastName"
    Then I expect the title of the page "View profile - UBORA"
    Then I expect the input "emailemail@email.com" of the element ".email" is visible
    Then I expect the input "TestName TestLastName" of the element ".fullname" is visible

Scenario: I sign in and click on my email on Members page
    When I click on the element "a=emailemail@email.com"
    Then I expect the title of the page "View members - UBORA"