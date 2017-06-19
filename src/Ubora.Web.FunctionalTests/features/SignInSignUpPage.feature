Feature: Home page
    As a developer
    I want to click buttons on SignInSignUp page

Background:
    Given I go to the website "Account/SignInSignUp"

Scenario: Create a project 
    Then I expect the title of the page "Sign in - UBORA"

Scenario: Click Logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Sign in/sign up
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in - UBORA"

Scenario: Click Continue with UBORA
    When I click on the element ".button.secondary-button.login-button"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Sign up
    When I click on the element ".button.primary-button.full-width"
    Then I expect the title of the page "Sign up - UBORA"