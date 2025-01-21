# Hermes Application

## 1. Definition of Done

### 1.1. Code Quality

- **Code Review:** All code must be reviewed by at least one other developer.
- **No Critical Bugs:** No known high-severity bugs in the completed work.

### 1.2. Functionality

- **Feature Requirements Met:** The feature or bug fix meets all acceptance criteria and requirements.
- **Integration:** The new code is integrated with existing systems and does not break existing functionality.

### 1.3. Documentation

- **Code Documentation:** Key functions and components are well-documented with comments.

### 1.4. Performance

- **Performance Testing:** The feature does not degrade application performance beyond acceptable limits.
- **Scalability Considerations:** Any scalability implications are considered and noted, even if not addressed immediately.

### 1.5. Security

- **Security Review:** Code is reviewed for common security vulnerabilities (e.g., SQL injection, XSS).
- **Data Handling:** Any data handled by the feature is done securely, with encryption or anonymization as needed.

### 1.6. Deployment

- **Build Passes:** The code passes all automated build processes.
- **Deployment Ready:** The code is ready to be deployed, with deployment scripts or instructions updated as needed.

### 1.7. Usability

- **User Experience Review:** The feature is reviewed from a usability perspective, ensuring it is intuitive and user-friendly.
- **Accessibility Considerations:** Basic accessibility standards are met (e.g., screen reader compatibility, color contrast).

## 2. Naming Strategy and Coding Guidelines

Our naming strategy and coding guidelines are strongly aligned with [Microsoft’s conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names).

### 2.1. General Practices

- **DRY Principle:** Avoid repeating code (Don’t Repeat Yourself).
- **KISS Principle:** Keep your code simple and straightforward (Keep It Simple, Stupid).
- **YAGNI Principle:** Don’t implement features until they are necessary (You Aren’t Gonna Need It).

### 2.2. Commenting

- **XML Documentation:** Use XML comments (`///`) for public members to provide descriptions that can be consumed by IntelliSense.
- **Inline Comments:** Use sparingly, only where the code is not self-explanatory.

### 2.3. Error Handling

- Use `try-catch` blocks for handling exceptions, and ensure you log exceptions properly.
- Avoid swallowing exceptions without handling them or rethrowing them.
- Use custom exceptions to provide more context when necessary.

### 2.4. Code Structure

- **Methods:** Methods should be short and focused on a single task. Ideally, a method should do one thing and do it well.
- **Classes:** Classes should be focused on a single responsibility (Single Responsibility Principle).
- **Regions:** Use `#region` for organizing large files, but avoid overusing them.

### 2.5. Dependency Injection

- Use Dependency Injection (DI) to manage dependencies between classes.
- Avoid static classes for services, as they make unit testing more difficult.

### 2.6. Asynchronous Programming

- Use `async` and `await` for IO-bound operations.
- Methods that perform asynchronous operations should end with `Async` (e.g., `GetCustomerAsync`).

### 2.7. LINQ

- Use LINQ for querying collections or databases where it improves readability.
- Avoid complex or deeply nested LINQ queries that reduce readability.

### 2.8. Security Practices

- Validate input data rigorously to prevent injection attacks.
- Store sensitive data securely (e.g., encryption for passwords).
- Use ASP.NET Identity for managing user authentication and authorization.

### 2.9. Unit Testing

- Write unit tests for all business logic.
- Use mock objects to isolate the unit of work being tested.
- Follow naming conventions for test methods: `MethodName_StateUnderTest_ExpectedBehavior`.

### 2.10. Database Access

- Use Entity Framework or Dapper for data access.
- Follow Repository and Unit of Work patterns to abstract data access.
- Avoid direct database calls from the Controller layer.

## 3. Testing Strategy

### 3.1. Test Cases

- **Arrange:** Set up the conditions for the test.
- **Act:** Execute the method or action being tested.
- **Assert:** Verify that the result matches expectations.

### 3.2. Test Coverage

- **MessageService:** Test sending, receiving, and formatting of messages.
- **UserService:** Test user registration, authentication, and session management.
- **ChatRoom:** Test chat room creation, joining, leaving, and message broadcasting.

### 3.3. Testing Goals and Objectives

- **Ensure Product Quality:** Detect bugs and issues early to prevent them from reaching production.
- **Enhance User Experience:** Validate that the product meets user expectations for functionality and usability.
- **Maintain Security and Compliance:** Ensure the product is secure and complies with relevant regulations.

### 3.4. Testing Environment

- **Development Environment:** Use MSTests for Unit testing.

### 3.5. Test Data Management

- **Test Data Creation:** Use anonymized production data or synthetic data to create realistic test cases.
- **Data Consistency:** Ensure that test data is consistent across environments to avoid false positives/negatives.

### 3.6. Reporting and Metrics

- **Test Coverage:** Measure and track test coverage, aiming for at least 80% coverage for critical paths.
- **Defect Density:** Track the number of defects per module or feature to identify areas needing improvement.
- **Cycle Time:** Measure the time taken to run tests and fix bugs to optimize the testing process.

## 4. Branching Strategy

### 4.1. Core Branches

#### 4.1.1. `main` Branch

- **Purpose:** The `main` branch contains production-ready code. It should always be stable and deployable.
- **Permissions:** Only allow merges into `main` after code has passed all tests and received necessary approvals.

#### 4.1.2. `develop` Branch

- **Purpose:** The `develop` branch contains the latest development changes that are ready for the next release but not yet in production.
- **Integration Point:** All feature branches are merged into `develop` after code reviews and successful testing.

### 4.2. Supporting Branches

#### 4.2.1. Feature Branches

- **Naming Convention:** `feature/name`
- **Purpose:** Feature branches are used to develop new features, enhancements, or experiments.
- **Lifecycle:** Created from `develop`, worked on, and merged back into `develop` when complete.
- **Best Practices:** Each feature branch should focus on a single feature or task.

### 4.3. Workflow

1. **Starting Work:**
   - Developers create feature branches from `develop` for new features or bugfix branches for bug fixes.
   - Regularly pull changes from `develop` to keep the branch updated.
2. **Pull Requests (PRs):**
   - When a feature or bugfix is complete, create a pull request (PR) to merge it into `develop`.
   - PRs should include code reviews and pass all automated tests before being merged.
3. **Merging:**
   - After approval and successful testing, merge the branch into `develop`.
   - Resolve any conflicts by rebasing or merging `develop` back into the feature branch before final merge.

### 4.4. Branch Protection Rules

- **`main` Branch:** Require PRs, code reviews, and passing CI checks before merging.
- **`develop` Branch:** Similar rules to `main` but allow faster merging to keep development moving.

### 4.5. CI/CD Integration

- **Automated Testing:** Ensure that all branches are tested automatically through CI pipelines. PRs should only be merged if they pass all tests.
- **Automatic Deployments:** Set up CI/CD to automatically deploy `develop` to a staging environment and `main` to production.

### 4.6. Branch Cleanup

- **Feature and Bugfix Branches:** Delete branches after they are merged into `develop`.
- **Release and Hotfix Branches:** Delete branches after they are merged into both `main` and `develop`.
