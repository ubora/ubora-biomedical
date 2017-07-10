Feature: Sign in Sign up page
    As a user
    I want to click all buttons on SignInSignUp page

Background:
    Given I go to the website "/Home/Index"
    Given I clicked on the element "a=Sign in/sign up"

Scenario: Go to SignInSignUp page 
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Sign in/sign up
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Sign up
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"

Scenario: Click Forgot password?
    When I click on the element "a=Forgot password?"
    Then I expect the title of the page "Forgot your password? - UBORA"